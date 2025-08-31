namespace Application.Dtos.ProcessDtos;

public class ProcessDto
{
    public Guid UserRoadmapId { get; set; }
    public Guid RoadmapElementId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsFinished { get; set; }
    public bool IsOpened { get; set; }
}
