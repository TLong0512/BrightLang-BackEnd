using Application.Dtos.QuestionBankService;
using Application.Dtos.RoadmapElementDtos;

namespace Application.Dtos.RoadmapDtos;

public class RoadmapDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public double QuestionPerDay { get; set; }
    public double TimeRequired { get; set; } // in days. 1 element = 1 day.
    public int SignupCount { get; set; }
    public LevelViewDto LevelStart { get; set; } = new LevelViewDto();
    public LevelViewDto LevelEnd { get; set; } = new LevelViewDto();

    public List<RoadmapElementDto> RoadmapElements { get; set; } = [];
}
