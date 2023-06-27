using System.Security.Claims;
using Application.Features.Offers;
using Application.Features.Offers.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OffersController : BaseController
{
    private readonly OffersService _offersService;

    public OffersController(OffersService offersService)
    {
        _offersService = offersService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOffer(CreateOfferRequest request)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var id = await _offersService.CreateOffer(userId, request);
        return Ok(await GetOfferById(id));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOfferById(Guid id)
    {
        var offer = await _offersService.GetOfferById(id);
        return Ok(offer);
    }
    
    [HttpGet("{publicationId:guid}/all")]
    public async Task<IActionResult> GetOffersByPublicationId(Guid publicationId)
    {
        var offers = await _offersService.GetOffersByPublicationId(publicationId);
        return Ok(offers);
    }

    [HttpGet("{publicationId:guid}/my")]
    public async Task<IActionResult> GetOffersByUserIdForPublication(Guid publicationId)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var offers = await _offersService.GetOffersByUserIdForPublication(userId, publicationId);
        return Ok(offers);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveOffer(Guid id)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        await _offersService.RemoveOffer(id, userId);
        return Ok();
    }
}