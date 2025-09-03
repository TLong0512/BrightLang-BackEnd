using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.RoadmapElementRangeDtos;

public class RoadmapElementRangeUpdateDto
{
    //public Guid RangeId { get; set; }

    [Required(ErrorMessage = "RepeatDays is required.")]
    [Range(1, 100, ErrorMessage = "RepeatDays must be between 1 and 100.")]
    public int RepeatDays { get; set; }


    [Required(ErrorMessage = "RepeatDays is required.")]
    [Range(1, 100, ErrorMessage = "QuestionPerDay must be between 1 and 100.")]
    public int QuestionPerDay { get; set; }
}
