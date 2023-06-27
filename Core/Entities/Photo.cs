namespace Core.Entities;

public class Photo : BaseEntity
{
    public string PhotoId { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; } = false;
}