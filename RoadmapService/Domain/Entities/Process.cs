namespace Domain.Entities;

public class Process : BaseEntity
{
    public Guid UserRoadmapId { get; set; }
    public Guid RoadmapElementId { get; set; }
    public bool IsFinished { get; set; }
    public bool IsOpened { get; set; }
}
