using Application.Dtos.ContextDtos;
using Application.Dtos.QuestionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface IQuestionService
    {
        public Task<IEnumerable<QuestionViewDto>> GellAllQuestionAsync();
        public Task<QuestionViewDto> GetQuestionByIdAsync(Guid id);
        public Task<Guid> AddQuestionAsync(QuestionAddDto questionAddDto);
        public Task<QuestionViewDto> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto);
        public Task<bool> DeleteQuestionAsync(Guid id);
        public Task<IEnumerable<QuestionViewDto>> GetQuestionsByContextIdAsync(Guid ContextId);
        public Task<IEnumerable<ContextDetailDto>> GenerateRandomQuestionInARangeByRangeId(Guid RangeId);
        public Task<bool> QuickAddQuestion(Guid SkillLevelId, IEnumerable<QuickQuestionAddDto> quickQuestionAddDtos);
    }
}
