using Core.Entities;

namespace Application.Features.AI.DTOs;

public class ReorderOffersDTO
{
    public string Content { get; set; }
    public string DesiredItem { get; set; }
    public IReadOnlyList<OfferDto> Offers { get; set; }
}