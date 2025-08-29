using Application.Dtos.AnswerDtos;
using Application.Services.Intefaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AnswerService : IAnswerService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;

        public AnswerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<bool> AddAnswerAsync(AnswerAddDto answerAddDto, Guid userId)
        {

            var existingQuestion = await _unitOfWork.QuestionRepository.GetByIdAsync(answerAddDto.QuestionId);
            if (existingQuestion == null)
            {
                return false;
            }
            else
            {
                var newAnswer = _mapper.Map<Answer>(answerAddDto);
                await _unitOfWork.AnswerRepository.AddAsync(newAnswer, userId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteAnswerAsync(Guid id)
        {
            var answer = await _unitOfWork.AnswerRepository.GetByIdAsync(id);

            if (answer == null)
            {
                return false;
            }
            else
            {
                await _unitOfWork.AnswerRepository.Delete(answer);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<AnswerViewDto>> GellAllAnswerAsync()
        {
            var result = await _unitOfWork.AnswerRepository.GetAllAnswersAsync();
            var listResultDto = _mapper.Map<IEnumerable<AnswerViewDto>>(result);
            return listResultDto;
        }

        public async Task<AnswerViewDto> GetAnswerByIdAsync(Guid id)
        {
            var result = await _unitOfWork.AnswerRepository.GetAnswerById(id);
            return _mapper.Map<AnswerViewDto>(result);
        }

        public async Task<IEnumerable<AnswerViewDto>> GetAnswerByQuestionIdAsync(Guid questionId)
        {
            var existingQuestion = await _unitOfWork.QuestionRepository.GetQuestionById(questionId);
            if(existingQuestion == null)
            {
                return null;
            }
            else
            {
                var result = await _unitOfWork.AnswerRepository.GetByConditionAsync(x => x.QuestionId == questionId);
                return _mapper.Map<IEnumerable<AnswerViewDto>>(result);
            }
        }

        public async Task<AnswerViewDto> UpdateAnswerAsync(Guid id, AnswerUpdateDto answerUpdateDto, Guid userId)
        {
            var answer = await _unitOfWork.AnswerRepository.GetAnswerById(id);
            if (answer == null)
            {
                return null;
            }
            else
            {

                var existingQuestion = await _unitOfWork.QuestionRepository.GetByIdAsync(answerUpdateDto.QuestionId);
                if (existingQuestion == null)
                {
                    return null;
                }
                else
                {
                    var updatedAnswer = _mapper.Map<Answer>(answerUpdateDto);
                    answer.QuestionId = updatedAnswer.QuestionId;
                    answer.Value = updatedAnswer.Value;
                    answer.IsCorrect = updatedAnswer.IsCorrect;
                    answer.Explain = updatedAnswer.Explain;
                    await _unitOfWork.AnswerRepository.Update(answer, userId);
                    await _unitOfWork.SaveChangesAsync();
                    return _mapper.Map<AnswerViewDto>(answer);
                }
            }
        }
    }
}
