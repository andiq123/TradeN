namespace Application.Features.Exchanges.Requests;

public class CreateExchangeRequest
{
    public Guid PublicationId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid OfferUserId { get; set; }
    public Guid OfferId { get; set; }

}