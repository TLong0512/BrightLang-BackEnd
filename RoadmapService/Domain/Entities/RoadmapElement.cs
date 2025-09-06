namespace Domain.Entities;

public class RoadmapElement : BaseEntity
{
    public Guid Id { get; set; }
    public Guid RoadmapId { get; set; }
    public int QuestionPerDay { get; set; }
    public int Order { get; set; }
    public Guid RangeId { get; set; }
}
