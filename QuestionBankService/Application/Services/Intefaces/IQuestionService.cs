using Application.Dtos.ContextDtos;
using Application.Dtos.QuestionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Intefaces
{
    public interface IQuestionService
    {
        public Task<IEnumerable<QuestionViewDto>> GellAllQuestionAsync();

        public Task<Guid> AddQuestionAsync(QuestionAddDto questionAddDto, Guid userId);
        public Task<QuestionViewDto> UpdateQuestionAsync(Guid id, QuestionUpdateDto questionUpdateDto, Guid userId);
        public Task<bool> DeleteQuestionAsync(Guid id);
        public Task<IEnumerable<QuestionViewDto>> GetQuestionsByContextIdAsync(Guid ContextId);
        public Task<bool> QuickAddQuestion(Guid skillId, Guid examTypeId, IEnumerable<QuickQuestionAddDto> quickQuestionAddDtos, Guid userId);
        public Task<QuestionSummaryDto> GetQuestionSummaryByIdAsync(Guid id);
        public Task<IEnumerable<QuestionSummaryDto>> GetAllQuestionSummaryByListIdAsync(List<Guid> QuestionIds);
        public Task<QuestionDetailDto> GetQuestionDetailByIdAsync(Guid id);
        public Task<IEnumerable<QuestionDetailDto>> GetAllQuestionDetailByListIdAsync(List<Guid> QuestionIds);
        public Task<IEnumerable<Guid>> GenerateAllExamTypeQuestionAsync(int numberPerSkillLevel = 2);
        public Task<IEnumerable<Guid>> GenerateQuestionByExamTypeIdAsync(Guid examTypeId, int numberPerSkillLevel = 2);
        public Task<IEnumerable<Guid>> GenerateQuestionByLevelIdAsync(Guid levelId, int numberPerSkillLevel = 2);
        public Task<IEnumerable<Guid>> GenerateQuestionBySkillLevelIdAsync(Guid skillLevelId, int number = 2);
        public Task<IEnumerable<Guid>> GenerateQuestionByRangeIdAsync(Guid rangeId);
    }
}
