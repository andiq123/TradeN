namespace Application.Features.Publications.Dtos;

public class ListPublicationsRequest
{
    public string? Title { get; set; }
    public OrderByEnum OrderBy { get; set; } = OrderByEnum.Date;
    public bool Descending { get; set; } = true;
}

public enum OrderByEnum
{
    Date,
    Title,
    // Add more properties for ordering if needed
}