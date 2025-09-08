using Application.Dtos.AnswerDtos;
using Application.Dtos.AnswerDtos.AnswerAddDtos;
using Application.Dtos.AnswerDtos.AnswerViewDtos;
using Application.Dtos.BaseDtos;
using Application.Dtos.ContextDtos;
using Application.Dtos.ContextDtos.ContestAddDto;
using Application.Dtos.QuestionDtos.QuestionAddDtos;
using Application.Dtos.QuestionDtos.QuestionUpdateDtos;
using Application.Dtos.QuestionDtos.QuestionViewDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IAnswerService _answerService;
        protected readonly IRangeService _rangeService;
        protected readonly IContextService _contextService;
        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IAnswerService answerService, IContextService contextService, IRangeService rangeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _answerService = answerService;
            _contextService = contextService;
            _rangeService = rangeService;
        }
        public async Task<Guid> AddQuestionAsync(QuestionAddDto questionAddDto, Guid userId)
        {

            var existingContext = await _unitOfWork.ContextRepository.GetContextById(questionAddDto.ContextId);
            if (existingContext == null)
            {
                throw new ArgumentException("Context not found");
            }

            if (questionAddDto.QuestionNumber < existingContext.Range.StartQuestionNumber
                || questionAddDto.QuestionNumber > existingContext.Range.EndQuestionNumber)
            {
                throw new ArgumentOutOfRangeException("Question number out of range");
            }

            if (existingContext.Questions.Select(x => x.QuestionNumber).Contains(questionAddDto.QuestionNumber))
            {
                throw new InvalidOperationException("Question is currently valid in its context");
            }

            var newQuestion = _mapper.Map<Question>(questionAddDto);

            await _unitOfWork.QuestionRepository.AddAsync(newQuestion, userId);
            await _unitOfWork.SaveChangesAsync();
            return newQuestion.Id;
        }
        public async Task<bool> QuickAddQuestion(Guid skillId, Guid examTypeId, IEnumerable<QuickQuestionAddDto> quickQuestionAddDtos, Guid userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var listRangeDto = await _rangeService.GetAllRangeIdByExamTypeAndSkill(examTypeId, skillId);

                foreach (var question in quickQuestionAddDtos)
                {
                    // check question belong to which range
                    var existingRange = listRangeDto.FirstOrDefault(x => x.StartQuestionNumber <= question.QuestionNumber
                                                            && x.EndQuestionNumber >= question.QuestionNumber);
                    //move to next question if it has wrong question number
                    if (existingRange == null)
                        throw new ArgumentException("a question has wrong number");
                    // check ansser allow one true 
                    if (question.AnswerList.Count(x => x.IsCorrect) != 1)
                            throw new ArgumentException("A question can only have one correct answer");
                    // check answer has unique value
                    if (question.AnswerList.Select(x => x.Value).Distinct().Count() != question.AnswerList.Count())
                        throw new ArgumentException("duplicate answer value");
                    //check context of question
                    var listContexts = await _unitOfWork.ContextRepository.GetContextByCondition(x => x.RangeId == existingRange.Id);
                    var existContext = listContexts.FirstOrDefault(x => x.Content != "" && x.Content == question.Context.Content);
                    //if content of context is "" or not exist in db => create new context
                    if (existContext == null)
                    {
                        var newContext = new Context
                        {
                            RangeId = existingRange.Id,
                            Content = question.Context.Content,
                            Explain = question.Context.Explain,
                            IsBelongTest = question.Context.IsBelongTest
                        };
                        await _unitOfWork.ContextRepository.AddAsync(newContext, userId);
                        existContext = newContext;
                    }
                    //check question number in question is exist in context
                    var listQuestionInContext = await _unitOfWork.QuestionRepository.GetByConditionAsync(x => x.ContextId == existContext.Id);
                    var listNumberInContext = listQuestionInContext.Select(x => x.QuestionNumber);
                    if (listNumberInContext.Any() && listNumberInContext.Contains(question.QuestionNumber))
                        throw new ArgumentException($"question {question.QuestionNumber} has been created in {existContext.Content}, check again");

                    var newQuestion = new Question
                    {
                        ContextId = existContext.Id,
                        Content = question.Content,
                        Explain = question.Explain,
                        QuestionNumber = question.QuestionNumber
                    };

                    await _unitOfWork.QuestionRepository.AddAsync(newQuestion, userId);

                    foreach (var answer in question.AnswerList)
                    {
                        var addAnswer = new Answer
                        {
                            QuestionId = newQuestion.Id,
                            Value = answer.Value,
                            Explain = answer.Explain,
                            IsCorrect = answer.IsCorrect
                        };
                        await _unitOfWork.AnswerRepository.AddAsync(addAnswer, userId);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }

        }
        public async Task<bool> DeleteQuestionAsync(Guid id)
        {
            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id);

            if (question == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.QuestionRepository.Delete(question);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }
        public async Task<PageResult<QuestionViewDto>> GellAllQuestionAsync(int page = 1, int pageSize = 10)
        {
            var result = _unitOfWork.QuestionRepository.GetAllQuestionsAsync();

            var totalItems = await result.CountAsync();

            var listTests = await result.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            var listResultDto = _mapper.Map<IEnumerable<QuestionViewDto>>(result);
            return new PageResult<QuestionViewDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = listResultDto
            };
        }
        public async Task<IEnumerable<QuestionDetailDto>> GetAllQuestionDetailByListIdAsync(List<Guid> QuestionIds)
        {
            List<QuestionDetailDto> results = new List<QuestionDetailDto>();
            foreach (var questionId in QuestionIds)
            {
                var questionDetail = await GetQuestionDetailByIdAsync(questionId);
                results.Add(questionDetail);
            }
            return results;
        }
        public async Task<IEnumerable<QuestionSummaryDto>> GetAllQuestionSummaryByListIdAsync(List<Guid> QuestionIds)
        {
            List<QuestionSummaryDto> results = new List<QuestionSummaryDto>();
            foreach (var questionId in QuestionIds)
            {
                var questionSummary = await GetQuestionSummaryByIdAsync(questionId);
                results.Add(questionSummary);
            }
            return results;
        }
        public async Task<QuestionDetailDto> GetQuestionDetailByIdAsync(Guid id)
        {
            var question = await _unitOfWork.QuestionRepository.GetQuestionById(id);
            var context = question.Context;
            var range = await _unitOfWork.RangeRepository.GetByIdAsync(context.RangeId);
            var skillLevel = await _unitOfWork.SkillLevelRepository.GetSkillLevelById(range.SkillLevelId);
            return new QuestionDetailDto
            {
                QuestionInformation = _mapper.Map<QuestionViewDto>(question),
                AnswerDetails = _mapper.Map<List<AnswerViewDto>>(question.Answers),
                ContextInformation = _mapper.Map<ContextViewDto>(context),
                LevelName = skillLevel.Level.Name,
                RangeName = range.Name,
                SkillName = skillLevel.Skill.SkillName
            };
        }
        public async Task<PageResult<QuestionViewDto>> GetQuestionsByRangeIdAsync(Guid rangeId, int page = 1, int pageSize = 10)
        {
            var listContextInRange = await _unitOfWork.ContextRepository.GetByConditionAsync(x => x.RangeId == rangeId);
            var listContextIds = listContextInRange.Select(x => x.Id);

            var questionsInContext = _unitOfWork.QuestionRepository.GetQuestionByConditionPaging(x => listContextIds.Contains(x.ContextId));

            var totalItems = await questionsInContext.CountAsync();

            questionsInContext = questionsInContext.OrderBy(x => x.QuestionNumber);

            var listTests = await questionsInContext.Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            var result = _mapper.Map<IEnumerable<QuestionViewDto>>(listTests);

            return new PageResult<QuestionViewDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = result
            };

        }
        public async Task<QuestionSummaryDto> GetQuestionSummaryByIdAsync(Guid id)
        {
            var question = await _unitOfWork.QuestionRepository.GetQuestionById(id);
            var range = await _unitOfWork.RangeRepository.GetByIdAsync(question.Context.RangeId);
            var skillLevel = await _unitOfWork.SkillLevelRepository.GetSkillLevelById(range.SkillLevelId);

            var answers = question.Answers;
            var answerSummaryDto = _mapper.Map<List<AnswerSummaryDto>>(answers);
            return new QuestionSummaryDto
            {
                QuestionNumber = question.QuestionNumber,
                Content = question.Content,
                AnswerContents = answerSummaryDto,
                ContextContent = question.Context.Content,
                SkillName = skillLevel.Skill.SkillName
            };
        }
        public async Task<QuestionViewDto> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto, Guid userId)
        {

            var question = await _unitOfWork.QuestionRepository.GetQuestionById(id);

            if (question == null)
            {
                throw new KeyNotFoundException("Not found question");
            }

            // check ansser allow one true 
            if (questionUpdateDto.ListAnswers.Count(x => x.IsCorrect) != 1)
                throw new ArgumentException("A question can only have one correct answer");
            // check answer has unique value
            if (questionUpdateDto.ListAnswers.Select(x => x.Value).Distinct().Count() != questionUpdateDto.ListAnswers.Count())
                throw new ArgumentException("duplicate answer value");

            // update context
            var context = question.Context;
            await _unitOfWork.ContextRepository.Delete(context);

            var existingRanges = await _unitOfWork.RangeRepository
                                                .GetByConditionAsync(x => x.StartQuestionNumber <= questionUpdateDto.QuestionNumber
                                                && x.EndQuestionNumber >= questionUpdateDto.QuestionNumber);
            var existingRange = existingRanges.FirstOrDefault();
            if (existingRange == null) throw new ArgumentException();

            var newContext = new Context();
            newContext.Content = questionUpdateDto.ContextUpdate.Content;
            newContext.Explain = questionUpdateDto.ContextUpdate.Explain;
            newContext.IsBelongTest = questionUpdateDto.ContextUpdate.IsBelongTest;
            newContext.RangeId = existingRange.Id;
            await _unitOfWork.ContextRepository.AddAsync(newContext, userId);
            await _unitOfWork.SaveChangesAsync();

            // update list answer
            foreach (var answer in question.Answers)
            {
                await _unitOfWork.AnswerRepository.Delete(answer);
            }

            foreach (var answerUpdateDto in questionUpdateDto.ListAnswers)
            {
                var updateAnswer = _mapper.Map<Answer>(answerUpdateDto);
                updateAnswer.QuestionId = question.Id;
                await _unitOfWork.AnswerRepository.AddAsync(updateAnswer, userId);
            }

            // update question
            question.ContextId = newContext.Id;
            question.QuestionNumber = questionUpdateDto.QuestionNumber;
            question.Content = questionUpdateDto.Content;
            question.Explain = questionUpdateDto.Explain;
            await _unitOfWork.QuestionRepository.Update(question, userId);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<QuestionViewDto>(question);
        }
        public async Task<IEnumerable<Guid>> GenerateAllExamTypeQuestionAsync(int numberPerSkillLevel = 2)
        {
            var listLevels = await _unitOfWork.LevelRepository.GetAllAsync();
            var listLevelIds = listLevels.Select(x => x.Id);
            List<Guid> listQuestionIdResult = new List<Guid>();
            foreach (var levelIds in listLevelIds)
            {
                var questionIds = await GenerateQuestionByLevelIdAsync(levelIds, numberPerSkillLevel);
                listQuestionIdResult.AddRange(questionIds);
            }
            return listQuestionIdResult;
        }
        public async Task<IEnumerable<Guid>> GenerateQuestionByExamTypeIdAsync(Guid examTypeId, int numberPerSkillLevel = 2)
        {
            var levels = await _unitOfWork.LevelRepository.GetByConditionAsync(x => x.ExamTypeId == examTypeId);
            List<Guid> listQuestionIds = new List<Guid>();
            foreach (var level in levels)
            {
                var listQuestionGenByLevels = await GenerateQuestionByLevelIdAsync(level.Id, numberPerSkillLevel);
                listQuestionIds.AddRange(listQuestionGenByLevels);
            }
            return listQuestionIds;
        }
        public async Task<IEnumerable<Guid>> GenerateQuestionByLevelIdAsync(Guid levelId, int numberPerSkillLevel = 2)
        {
            var skills = await _unitOfWork.SkillRepository.GetAllAsync();
            // only get reading and listening skill
            var skillIds = skills.Select(x => x.Id);
            var skillLevels = await _unitOfWork.SkillLevelRepository.GetByConditionAsync(x => x.LevelId == levelId && skillIds.Contains(x.SkillId));
            List<Guid> listQuestionIds = new List<Guid>();
            foreach (var skillLevel in skillLevels)
            {
                var listQuestionGenBySkillLevels = await GenerateQuestionBySkillLevelIdAsync(skillLevel.Id, numberPerSkillLevel);
                listQuestionIds.AddRange(listQuestionGenBySkillLevels);
            }
            return listQuestionIds;
        }
        public async Task<IEnumerable<Guid>> GenerateQuestionBySkillLevelIdAsync(Guid skillLevelId, int number = 2)
        {
            var listExistRanges = await _unitOfWork.RangeRepository.GetByConditionAsync(x => x.SkillLevelId == skillLevelId);
            List<Guid> result = new List<Guid>();
            Random random = new Random();
            foreach (var range in listExistRanges)
            {
                var listGenQuestion = await GenerateQuestionByRangeIdAsync(range.Id);
                result.AddRange(listGenQuestion);
            }

            if (number > 0 && result.Count > number)
            {
                result = result
                    .OrderBy(x => random.Next())
                    .Take(number)
                    .ToList();
            }

            return result;
        }
        public async Task<IEnumerable<Guid>> GenerateQuestionByRangeIdAsync(Guid rangeId)
        {
            var existingRange = await _unitOfWork.RangeRepository.GetByIdAsync(rangeId);
            //get list context in range
            var listContext = await _unitOfWork.ContextRepository.GetContextByCondition(x => x.RangeId == rangeId);
            //gel list question belong to context in list context
            var listQuestions = listContext.SelectMany(c => c.Questions).ToList();

            //generate random question order by question number in a range
            List<Guid> listGenQuestionResult = new List<Guid>();
            Random random = new Random();

            for (int i = existingRange.StartQuestionNumber; i <= existingRange.EndQuestionNumber; ++i)
            {
                var listCompatibleNumberQuestions = listQuestions.Where(x => x.QuestionNumber == i).ToList();

                //move to next question number if in db doesnt has any kind of question
                if (!listCompatibleNumberQuestions.Any())
                    continue;

                int randomIndex = random.Next(listCompatibleNumberQuestions.Count());
                var randomQuestion = listCompatibleNumberQuestions.ElementAt(randomIndex);

                listGenQuestionResult.Add(randomQuestion.Id);

                if (i < existingRange.EndQuestionNumber - 1
                  && listQuestions.Any(x => x.QuestionNumber == i + 1 && x.ContextId == randomQuestion.ContextId))
                {
                    var coupleQuestion = listQuestions.First(x => x.QuestionNumber == i + 1 && x.ContextId == randomQuestion.ContextId);
                    listGenQuestionResult.Add(coupleQuestion.Id);
                    i++;
                }
            }
            return listGenQuestionResult;
        }
    }
}
