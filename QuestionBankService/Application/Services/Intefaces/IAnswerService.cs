using Application.Dtos.AnswerDtos.AnswerAddDtos;
using Application.Dtos.AnswerDtos.AnswerUpdateDtos;
using Application.Dtos.AnswerDtos.AnswerViewDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface IAnswerService
    {
        public Task<IEnumerable<AnswerViewDto>> GellAllAnswerAsync();
        public Task<AnswerViewDto> GetAnswerByIdAsync(Guid id);
        public Task<bool> AddAnswerAsync(AnswerAddDto AnswerAddDto, Guid userId);
        public Task<AnswerViewDto> UpdateAnswerAsync(Guid id, AnswerUpdateDto AnswerUpdateDto, Guid userId);
        public Task<bool> DeleteAnswerAsync(Guid id);
        public Task<IEnumerable<AnswerViewDto>> GetAnswerByQuestionIdAsync(Guid questionId);
    }
}
