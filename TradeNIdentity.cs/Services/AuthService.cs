using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Identity;
using Application.Exceptions;
using Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TradeNIdentity.cs.Models;

namespace TradeNIdentity.cs.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserService _userService;
    private readonly JwtSettings _jwtSettings;

    public AuthService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IOptions<JwtSettings> jwtSettings, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            throw new BadRequestException("Email already in use");
        }

        var userId = Guid.NewGuid();
        var newUser = new IdentityUser()
        {
            Id = userId.ToString(),
            Email = request.Email,
            UserName = request.Username
        };

        var isCreated = await _userManager.CreateAsync(newUser, request.Password);

        if (!isCreated.Succeeded)
        {
            var formattedErrors = isCreated.Errors.Select(x => x.Description);
            throw new BadRequestException("Unable to create user, errorList : " + string.Join(",", formattedErrors));
        }
        
        await _userService.CreateUser(userId, request);

        var token = GenerateToken(userId.ToString(), request.Username);

        return new AuthResponse()
        {
            Id = userId.ToString(),
            Token = token,
            Username = request.Username,
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user is null)
        {
            throw new NotFoundException("Invalid Email or Password");
        }

        var isCorrectPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isCorrectPassword)
        {
            throw new NotFoundException("Invalid Email or Password");
        }

        var token = GenerateToken(user.Id, user.UserName);

        return new AuthResponse()
        {
            Id = user.Id,
            Token = token,
            Username = user.UserName,
        };
    }


    private string GenerateToken(string id, string userName)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Audience = _jwtSettings.Audience,
            Issuer = _jwtSettings.Issuer,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                //claim for ID
                new Claim(ClaimTypes.NameIdentifier, id),
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}