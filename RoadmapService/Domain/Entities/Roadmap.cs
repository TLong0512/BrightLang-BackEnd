namespace Domain.Entities;

public class Roadmap : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid LevelStartId { get; set; }
    public Guid LevelEndId { get; set; }
}
