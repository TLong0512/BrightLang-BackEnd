using Application.Dtos.QuestionBankService;

namespace Application.Dtos.RoadmapElementDtos;

public class RoadmapElementDto
{
    public Guid Id { get; set; }
    public Guid RoadmapId { get; set; }
    public int QuestionPerDay { get; set; }
    public int Order { get; set; }

    //public Guid RangeId { get; set; }
    public RangeViewDto Range { get; set; } = new RangeViewDto();
}
