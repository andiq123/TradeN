using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Application.Features.Publications.Dtos;

public class CreatePublicationRequest
{
    [Required] [MinLength(5)] public string Title { get; set; }
    [Required] [MinLength(5)] public string Content { get; set; }
    public string ContentResumed { get; set; }
    public string DesiredItem { get; set; }
    public IList<Photo>? Photos { get; set; } = new List<Photo>();
}