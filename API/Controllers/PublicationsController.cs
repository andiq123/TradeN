using System.Security.Claims;
using Application.Features.Publications;
using Application.Features.Publications.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PublicationsController : BaseController
{
    private readonly PublicationService _publicationService;

    public PublicationsController(PublicationService publicationService)
    {
        _publicationService = publicationService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPublicationById(Guid id)
    {
        var publication = await _publicationService.GetPublicationById(id);
        return Ok(publication);
    }

    [HttpGet]
    public async Task<IActionResult> GetPublications([FromQuery] ListPublicationsRequest request)
    {
        var publications = await _publicationService.GetPublications(request);
        return Ok(publications);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyPublications()
    {
        //get logged user Id
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var publications = await _publicationService.GetPublications(userId);
        return Ok(publications);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePublication(CreatePublicationRequest request)
    {
        //get logged user Id
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var id = await _publicationService.CreatePublication(userId, request);
        return Ok(await GetPublicationById(id));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemovePublication(Guid id)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

        await _publicationService.RemovePublication(id, userId);
        return Ok();
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePublication(Guid id, [FromBody] UpdatePublicationRequest request)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

        await _publicationService.UpdatePublication(id, userId, request);
        return Ok();
    }
}