namespace Core.Entities;

public class Exchange : BaseEntity
{
    public Guid PublicationId { get; set; }
    public Publication Publication { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    public Guid OfferUserId { get; set; }
    public User OfferUser { get; set; }

    public Guid OfferId { get; set; }
    public Offer Offer { get; set; }
    
    public DateTime ExchangeDate { get; set; }
    
}