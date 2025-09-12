using Application.Dtos.ProcessDtos;
using Application.Dtos.RoadmapDtos;

namespace Application.Dtos.UserRoadmapDtos;

public class UserRoadmapDetailDto
{
    public Guid Id { get; set; }
    public RoadmapPreviewDto Roadmap { get; set; } = new();
    public List<ProcessDetailDto> Processs { get; set; } = [];
}
