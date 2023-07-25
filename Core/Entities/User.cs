namespace Core.Entities;

public class User : BaseEntity
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string FullName { get; init; }
    public string Location { get; init; }
    public string PhoneNumber { get; init; }
    public string? PhotoUrl { get; set; }
    public double Rating { get; set; } = 5.0f;
    public ICollection<Publication> Publications { get; init; }
    public ICollection<Offer> Offers { get; init; }
}