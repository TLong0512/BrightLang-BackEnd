using Application.Dtos.QuestionBankService;

namespace Application.Abstraction.Services;

public interface IQuestionbankService
{
    Task<List<LevelViewDto>> GetAllLevelsAsync();
    Task<LevelViewDto?> GetLevelByIdAsync(Guid id);


    Task<List<SkillLevelViewDto>> GetAllSkillLevelsAsync();
    Task<SkillLevelViewDto?> GetSkillLevelByIdAsync(Guid id);


    Task<List<RangeViewDto>> GetAllRangesAsync();
    Task<List<RangeViewDto>?> GetAllRangesBySkillLevelIdAsync(Guid skillLevelId);
    Task<RangeViewDto?> GetRangeByIdAsync(Guid id);
}
