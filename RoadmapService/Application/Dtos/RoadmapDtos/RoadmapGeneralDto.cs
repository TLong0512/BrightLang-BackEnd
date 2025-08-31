using Application.Dtos.QuestionBankService;

namespace Application.Dtos.RoadmapDtos;

public class RoadmapGeneralDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LevelViewDto LevelStart { get; set; } = new LevelViewDto();
    public LevelViewDto LevelEnd { get; set; } = new LevelViewDto();
}
