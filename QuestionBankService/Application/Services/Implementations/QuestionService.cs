using Application.Dtos.AnswerDtos;
using Application.Dtos.ContextDtos;
using Application.Dtos.QuestionDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
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
        protected readonly IContextService _contextService;
        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper, IAnswerService answerService, IContextService contextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _answerService = answerService;
            _contextService = contextService;
        }

        public async Task<Guid> AddQuestionAsync(QuestionAddDto questionAddDto)
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
                var newQuestion = _mapper.Map<Question>(questionAddDto);
                await _unitOfWork.QuestionRepository.AddAsync(newQuestion, new Guid());
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
            foreach (var entity in listResultDto)
            {
                entity.AnswerViewDtos = await _answerService.GetAnswerByQuestionIdAsync(entity.Id);
            }
            return listResultDto;
        }

        public async Task<IEnumerable<ContextDetailDto>> GenerateRandomQuestionInARangeByRangeId(Guid rangeId)
        {
            var existingRange = await _unitOfWork.RangeRepository.GetByIdAsync(rangeId);
            if (existingRange == null)
            {
                return null;
            }
            else
            {
                List<ContextDetailDto> contextDetailDtos = new List<ContextDetailDto>();
                for (int i = existingRange.StartQuestionNumber; i < existingRange.EndQuestionNumber; ++i)
                {
                    var listQuestionByQuestionNumber = await _unitOfWork.QuestionRepository.GetQuestionByCondition(x => x.QuestionNumber == i);
                    Random random = new Random();
                    int id = random.Next(listQuestionByQuestionNumber.Count());
                    var randomQuestion = listQuestionByQuestionNumber.ElementAt(id);

                    var context = randomQuestion.Context;
                    var listQuestionInContext = await GetQuestionsByContextIdAsync(context.Id);
                    ContextDetailDto contextDetailDto = new ContextDetailDto
                    {
                        ContextViewDto = _mapper.Map<ContextViewDto>(context),
                        QuestionViewDtos = listQuestionInContext
                    };
                    contextDetailDtos.Add(contextDetailDto);
                    if (listQuestionInContext.Count() == existingRange.EndQuestionNumber - 1)
                    {
                        break;
                    }
                    i += listQuestionInContext.Count() - 1;
                }
                return contextDetailDtos;
            }
        }

        public async Task<QuestionViewDto> GetQuestionByIdAsync(Guid id)
        {
            var result = await _unitOfWork.QuestionRepository.GetQuestionById(id);
            var resultDto = _mapper.Map<QuestionViewDto>(result);
            resultDto.AnswerViewDtos = await _answerService.GetAnswerByQuestionIdAsync(resultDto.Id);
            return resultDto;
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

        public async Task<bool> QuickAddQuestion(Guid skillLevelId, IEnumerable<QuickQuestionAddDto> quickQuestionAddDtos)
        {
            var existingSkillLevel = await _unitOfWork.SkillLevelRepository.GetByIdAsync(skillLevelId);
            if(existingSkillLevel == null)
            {
                return false;
            }
            else
            {
                var listRange = await _unitOfWork.RangeRepository.GetByConditionAsync(x => x.SkillLevelId == skillLevelId);
                foreach (var entity in quickQuestionAddDtos)
                {
                    var range = listRange.FirstOrDefault(x => x.StartQuestionNumber <= entity.QuestionNumber && x.EndQuestionNumber >= entity.QuestionNumber);
                    if(range == null|| entity.QuickContextAddDto.Content == null)
                    {
                        continue;
                    }
                    else
                    {
                        var listContext = await _unitOfWork.ContextRepository.GetByConditionAsync(x => x.RangeId == range.Id);
                        var existContext = listContext.FirstOrDefault(x => x.Content == entity.QuickContextAddDto.Content);
                        if(existContext == null)
                        {
                            ContextAddDto contextAddDto = new ContextAddDto
                            {
                                RangeId = range.Id,
                                Content = entity.QuickContextAddDto.Content,
                                Explain = entity.QuickContextAddDto.Explain,
                                IsBelongTest = entity.QuickContextAddDto.IsBelongTest
                            };
                            var newContextId = await _contextService.AddContextAsync(contextAddDto);
                            existContext = await _unitOfWork.ContextRepository.GetContextById(newContextId);
                        }

                        var questionAddDto = new QuestionAddDto
                        {
                            ContextId = existContext.Id,
                            Content = entity.Content,
                            Explain = entity.Explain,
                            QuestionNumber = entity.QuestionNumber
                        };
                        var newQuestionAddId = await AddQuestionAsync(questionAddDto);
                        foreach(var answer in entity.QuickAnswerAddDtos)
                        {
                            var answerAddDto = new AnswerAddDto
                            {
                                QuestionId = newQuestionAddId,
                                Value = answer.Value,
                                Explain = answer.Explain,
                                IsCorrect = answer.IsCorrect
                            };
                            await _answerService.AddAnswerAsync(answerAddDto);
                        }

                    }

                }
                return true;
            }
        }

        public async Task<QuestionViewDto> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto)
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
                    await _unitOfWork.QuestionRepository.Update(question, new Guid());
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<QuestionViewDto>(question);
                }
            }
        }
    }
}
