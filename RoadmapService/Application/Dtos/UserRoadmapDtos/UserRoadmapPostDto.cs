using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.UserRoadmapDtos;

public class UserRoadmapPostDto
{
    [Required(ErrorMessage = "Roadmap id is required.")]
    public Guid RoadmapId { get; set; }
}
