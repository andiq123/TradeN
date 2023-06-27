using System.Security.Claims;
using Application.Features.Exchanges;
using Application.Features.Exchanges.Requests;
using Application.Features.Publications;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class ExchangesController : BaseController
{
    private readonly ExchangeService _exchangeService;
    private readonly PublicationService _publicationService;

    public ExchangesController(ExchangeService exchangeService, PublicationService publicationService)
    {
        _exchangeService = exchangeService;
        _publicationService = publicationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateExchangeAsync(CreateExchangeRequest request)
    {
        var exchange = await _exchangeService.CreateExchangeAsync(request);
        await _publicationService.SetPublicationNotAvailable(request.PublicationId);
        return Ok(exchange);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExchangeByIdAsync(Guid id)
    {
        var exchange = await _exchangeService.GetExchangeByIdAsync(id);
        return Ok(exchange);
    }

    [HttpGet("my-exchanges")]
    public async Task<IActionResult> GetMyExchangesAsync()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var exchanges = await _exchangeService.GetMyExchangesAsync(userId);
        return Ok(exchanges);
    }
}