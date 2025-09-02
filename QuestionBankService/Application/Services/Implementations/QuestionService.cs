using Application.Dtos.AnswerDtos;
using Application.Dtos.ContextDtos;
using Application.Dtos.QuestionDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return Guid.Empty;
            }
            else
            {
                if (questionAddDto.QuestionNumber < existingContext.Range.StartQuestionNumber
                    || questionAddDto.QuestionNumber > existingContext.Range.EndQuestionNumber)
                {
                    return Guid.Empty;
                }

                var context = await _unitOfWork.ContextRepository.GetContextById(questionAddDto.ContextId);
                if (context.Questions.Select(x => x.QuestionNumber).Contains(questionAddDto.QuestionNumber))
                {
                    return Guid.Empty;
                }

                var newQuestion = _mapper.Map<Question>(questionAddDto);
                await _unitOfWork.QuestionRepository.AddAsync(newQuestion, userId);
                await _unitOfWork.SaveChangesAsync();
                return newQuestion.Id;
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
        public async Task<IEnumerable<QuestionViewDto>> GellAllQuestionAsync()
        {
            var result = await _unitOfWork.QuestionRepository.GetAllQuestionsAsync();
            var listResultDto = _mapper.Map<IEnumerable<QuestionViewDto>>(result);
            return listResultDto;
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
            foreach(var questionId in QuestionIds)
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

        public async Task<IEnumerable<QuestionViewDto>> GetQuestionsByContextIdAsync(Guid ContextId)
        {
            var existingContext = await _unitOfWork.ContextRepository.GetContextById(ContextId);
            if (existingContext == null)
            {
                return null;
            }
            else
            {
                var questionsInContext = await _unitOfWork.QuestionRepository.GetByConditionAsync(x => x.ContextId == ContextId);
                var orderQuestionInContext = questionsInContext.OrderBy(x => x.QuestionNumber);
                return _mapper.Map<IEnumerable<QuestionViewDto>>(orderQuestionInContext);
            }
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
        public async Task<bool> QuickAddQuestion(Guid skillId, Guid examTypeId, IEnumerable<QuickQuestionAddDto> quickQuestionAddDtos, Guid userId)
        {
            var listRangeDto = await _rangeService.GetAllRangeIdByExamTypeAndSkill(examTypeId, skillId);

            foreach (var question in quickQuestionAddDtos)
            {
                // check question belong to which range
                var existingRange = listRangeDto.FirstOrDefault(x => x.StartQuestionNumber <= question.QuestionNumber
                                                        && x.EndQuestionNumber >= question.QuestionNumber);
                //move to next question if it has wrong question number
                if (existingRange == null)
                    continue;
                //check context of question
                var listContexts = await _unitOfWork.ContextRepository.GetContextByCondition(x => x.RangeId == existingRange.Id);
                var existContext = listContexts.FirstOrDefault(x => x.Content != "" && x.Content == question.Context.Content);
                //if content of context is "" or not exist in db => create new context
                if (existContext == null)
                {
                    var contextAddDto = new ContextAddDto
                    {
                        RangeId = existingRange.Id,
                        Content = question.Context.Content,
                        Explain = question.Context.Explain,
                        IsBelongTest = question.Context.IsBelongTest
                    };
                    var newContextId = await _contextService.AddContextAsync(contextAddDto, userId);
                    existContext = await _unitOfWork.ContextRepository.GetContextById(newContextId);
                }
                //check question number in question is exist in context
                var questionNumberInContext = existContext.Questions.Select(x => x.QuestionNumber);
                if (questionNumberInContext.Contains(question.QuestionNumber))
                    continue;
                var questionAddDto = new QuestionAddDto
                {
                    ContextId = existContext.Id,
                    Content = question.Content,
                    Explain = question.Explain,
                    QuestionNumber = question.QuestionNumber
                };
                var newQuestionAddId = await AddQuestionAsync(questionAddDto, userId);

                foreach (var answer in question.AnswerList)
                {
                    var answerAddDto = new AnswerAddDto
                    {
                        QuestionId = newQuestionAddId,
                        Value = answer.Value,
                        Explain = answer.Explain,
                        IsCorrect = answer.IsCorrect
                    };
                    await _answerService.AddAnswerAsync(answerAddDto, userId);
                }

            }
            return true;
        }
        public async Task<QuestionViewDto> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto, Guid userId)
        {
            var question = await _unitOfWork.QuestionRepository.GetQuestionById(id);
            if (question == null)
            {
                return null;
            }
            else
            {

                var existingContext = await _unitOfWork.ContextRepository.GetByIdAsync(questionUpdateDto.ContextId);
                if (existingContext == null)
                {
                    return null;
                }
                else
                {
                    var updatedQuestion = _mapper.Map<Question>(questionUpdateDto);
                    question.ContextId = updatedQuestion.ContextId;
                    question.QuestionNumber = updatedQuestion.QuestionNumber;
                    question.Content = updatedQuestion.Content;
                    question.Explain = updatedQuestion.Explain;
                    await _unitOfWork.QuestionRepository.Update(question, userId);
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<QuestionViewDto>(question);
                }
            }
        }
        public async Task<IEnumerable<Guid>> GenerateAllExamTypeQuestionAsync(int numberPerSkillLevel = 2)
        {
            var listLevels = await _unitOfWork.LevelRepository.GetAllAsync();
            var listLevelIds = listLevels.Select(x => x.Id);
            List<Guid> listQuestionIdResult = new List<Guid>();
            foreach(var levelIds in listLevelIds)
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
