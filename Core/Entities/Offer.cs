namespace Core.Entities;

public class Offer : BaseEntity
{
    public string Title { get; init; }
    public string Content { get; init; }
    public DateTime OfferDate { get; init; }

    public int Rank { get; set; } = 0;
    public Guid PublicationId { get; set; }
    public Publication Publication { get; init; }

    public Guid UserId { get; set; }
    public User User { get; init; }
    public ICollection<Photo> Photos { get; set; }
}