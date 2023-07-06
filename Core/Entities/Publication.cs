namespace Core.Entities;

public class Publication : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string? ContentResumed { get; set; }
    public string DesiredItem { get; set; }
    public bool IsAvailable { get; set; }
    public DateTime PublishDate { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<Photo> Photos { get; set; } 
    public ICollection<Offer> Offers { get; set; }
}