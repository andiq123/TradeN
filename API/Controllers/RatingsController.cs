using System.Linq.Expressions;
using System.Security.Claims;
using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RatingRequest
{
    public Guid ForUserId { get; set; }
    public Guid ForPublicationId { get; set; }
    
    public float Rate { get; set; } = 5f;
}

[Authorize]
public class RatingsController : BaseController
{
    private readonly IGenericRepository<Rating> _ratingRepo;
    private readonly IUserService _userService;

    public RatingsController(IGenericRepository<Rating> ratingRepo, IUserService userService)
    {
        _ratingRepo = ratingRepo;
        _userService = userService;
    }


    [HttpPost]
    public async Task<IActionResult> SetRating(RatingRequest request)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var rating = new Rating()
            { ForPublicationId = request.ForPublicationId, ForUserId = request.ForUserId, FromUserId = userId,Rate = request.Rate };

        await _ratingRepo.AddAsync(rating);

        Expression<Func<Rating, bool>> where = x => x.ForUserId == request.ForUserId;

        var ratings = await _ratingRepo.ListAllAsync(where);
        var rate = ratings.Select(x => x.Rate).Average();

        if (rate == 0)
        {
            rate = 0.5f;
        }

        await _userService.SetRating(request.ForUserId, rate);
        return NoContent();
    }

    [HttpGet("{publicationId:guid}")]
    public async Task<IActionResult> CheckIfAvailableForRate(Guid publicationId)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var rating =
            await _ratingRepo.GetFirstOrDefault(x =>
                x.ForPublicationId == publicationId && x.FromUserId == userId);

        var result = rating is not null ? true : false;
        return Ok(new { alreadyRated = result });
    }
}