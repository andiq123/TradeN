using System.Security.Claims;
using Application.Contracts.Identity;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var response = await _userService.GetUserById(userId);
        return Ok(response);
    }
    
    
}