namespace Application.Features.AI.DTOs;

public class OfferDto
{
    public Guid OfferId { get; set; }
    public string Content { get; set; }
    public int Rank { get; set; } = 0;
}