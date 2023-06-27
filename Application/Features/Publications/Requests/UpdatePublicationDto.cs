using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace Application.Features.Publications.Dtos;

public class UpdatePublicationRequest
{
    [Required] [MinLength(5)] public string Title { get; init; }
    [Required] [MinLength(5)] public string Content { get; init; }
    public IList<Photo>? Photos { get; set; } = new List<Photo>();
}