using Core.Entities;

namespace Application.Features.Offers.Requests;

public class CreateOfferRequest
{
    public string Title { get; set; }
    public string Content { get; init; }

    public string PublicationId { get; set; }
    
    public IList<Photo>? Photos { get; set; } = new List<Photo>();
}