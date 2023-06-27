using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public class RegisterRequest
{
    [Required]
    [MinLength(4)]
    public string Username { get; init; }
    [Required]
    [MinLength(4)]
    public string Password { get; init; }
    [Required]
    [MinLength(4)]
    [EmailAddress]
    public string Email { get; init; }
    [Required]
    [MinLength(4)]
    public string FullName { get; init; }
    [Required]
    [MinLength(4)]
    public string Location { get; init; }
    [Required]
    [MinLength(4)]
    public string PhoneNumber { get; init; }
}