namespace Domain.Entities;

public class UserRoadmap : BaseEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RoadmapId { get; set; }
}
