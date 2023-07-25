namespace Core.Entities;

public class Rating : BaseEntity
{
    public Guid ForUserId { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ForPublicationId { get; set; }
    public float Rate { get; set; } = 0.5f;
}