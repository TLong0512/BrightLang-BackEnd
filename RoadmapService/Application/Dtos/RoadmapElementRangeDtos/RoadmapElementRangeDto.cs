using Application.Dtos.QuestionBankService;

namespace Application.Dtos.RoadmapElementRangeDtos;

public class RoadmapElementRangeDto
{
    public RangeViewDto Range { get; set; } = new RangeViewDto();
    public int RepeatDays { get; set; }
    public int QuestionPerDay { get; set; }
}
