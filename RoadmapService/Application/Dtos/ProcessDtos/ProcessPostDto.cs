using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.ProcessDtos;

public class ProcessPostDto
{
    [Required(ErrorMessage = "UserRoadmap Id is required.")]
    public Guid UserRoadmapId { get; set; }


    [Required(ErrorMessage = "UserRoadmap Id is required.")]
    public Guid RoadmapElementId { get; set; }


    [Required(ErrorMessage = "UserRoadmap Id is required.")]
    public DateOnly StartDate { get; set; }


    [Required(ErrorMessage = "UserRoadmap Id is required.")]
    public DateOnly EndDate { get; set; }


    [Required(ErrorMessage = "UserRoadmap Id is required.")]
    public bool IsFinished { get; set; }


    [Required(ErrorMessage = "UserRoadmap Id is required.")]
    public bool IsOpened { get; set; }
}
