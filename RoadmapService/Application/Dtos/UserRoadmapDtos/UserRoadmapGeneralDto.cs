using Application.Dtos.ProcessDtos;
using Application.Dtos.RoadmapDtos;

namespace Application.Dtos.UserRoadmapDtos;

public class UserRoadmapGeneralDto
{
    public Guid Id { get; set; }
    public RoadmapGeneralDto Roadmap { get; set; } = new RoadmapGeneralDto();
    public ProcessStatisticDto ProcessStatistic { get; set; } = new();
}
