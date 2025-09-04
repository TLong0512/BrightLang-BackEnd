using Application.Dtos.ExamTypeDto;
using Application.Dtos.LevelDto;
using Application.Dtos.QuestionDto;
using Application.Dtos.RageDto;
using Application.Dtos.SkillLevelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IQuestionService
    {
        Task<IEnumerable<ExamTypeViewDto>> GetAllExamTypesAsync();
        Task<IEnumerable<LevelViewDto>> GetLevelsByExamTypeIdAsync(Guid examTypeId);
        Task<IEnumerable<SkillLevelViewDto>> GetSkillLevelByLevelIdAsync(Guid levelId);
        Task<IEnumerable<RangeViewDto>> GetRangesBySkillLevelIdAsync(Guid skillLevelId);
        Task<IEnumerable<QuestionSummaryDto>> GenerateQuestionsByRangeIdAsync(Guid rangeId);
        Task<IEnumerable<QuestionDetailDto>> ShowDetailQuestionsByRangeIdAsync(Guid rangeId);
    }
}
