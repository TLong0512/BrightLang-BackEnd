using Application.Dtos.QuestionBankService;
using Application.Dtos.RoadmapElementDtos;

namespace Application.Dtos.RoadmapDtos;

public class RoadmapSpecificDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LevelViewDto LevelStart { get; set; } = new LevelViewDto();
    public LevelViewDto LevelEnd { get; set; } = new LevelViewDto();
    public List<RoadmapElementDto> RoadmapElements { get; set; } = [];
}
