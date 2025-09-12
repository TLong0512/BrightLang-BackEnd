namespace Application.Dtos.ProcessDtos;

public class ProcessDetailDto
{
    // in case you need more.
    public Guid RoadmapElementId { get; set; }


    // roadmap element
    public int QuestionPerDay { get; set; }
    public Guid RangeId { get; set; }


    // process
    public bool IsFinished { get; set; }
    public bool IsOpened { get; set; }
}
