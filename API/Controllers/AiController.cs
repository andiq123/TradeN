using System.Text;
using System.Text.RegularExpressions;
using Application.Features.AI;
using Application.Features.AI.DTOs;
using Application.Features.AI.Requests;
using Application.Features.Offers;
using Application.Features.Offers.Requests;
using Application.Features.Publications;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API.Controllers;

public class AiController : BaseController
{
    private readonly AiService _aiService;
    private readonly PublicationService _publicationService;
    private readonly OffersService _offersService;

    public AiController(AiService aiService, PublicationService publicationService, OffersService offersService)
    {
        _aiService = aiService;
        _publicationService = publicationService;
        _offersService = offersService;
    }

    [HttpPost]
    public async Task<IActionResult> Query([FromBody] ResumeRequest request)
    {
        return Ok(await _aiService.ResumeContent(request.Query));
    }

    [HttpGet("{publicationId:guid}")]
    public async Task<IActionResult> Reorder(Guid publicationId)
    {
        var publication = await _publicationService.GetPublicationById(publicationId);
        var offers = await _offersService.GetOffersByPublicationId(publicationId);

        var offersDto = offers.Select(x => new OfferDto() { OfferId = x.Id, Content = x.Content, Rank = 0 }).ToList();
        var reorderOffersDto = new ReorderOffersDTO()
        {
            Content = publication.Content,
            DesiredItem = publication.DesiredItem,
            Offers = offersDto
        };

        var result = await _aiService.ReorderOffers(reorderOffersDto);
        var pattern = @"\{(?:[^{}]|(?<open>\{)|(?<close-open>\}))+(?(open)(?!))\}";

        var matches = Regex.Matches(result, pattern);
        if (matches.Count == 0)
        {
            return BadRequest("Could not reorder offers");
        }

        var requests = new List<ReorderRequest>();

        foreach (Match match in matches)
        {
            string json = match.Value;

            var byteArray = Encoding.UTF8.GetBytes(json);
            using var memoryStream = new MemoryStream(byteArray);
            var request = await JsonSerializer.DeserializeAsync<ReorderRequest>(memoryStream);
            requests.Add(request);
        }

        await _offersService.SetRank(requests);

        return Ok();
    }
}