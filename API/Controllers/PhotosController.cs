using System.Security.Claims;
using Application.Contracts.Cloudinary;
using Application.Features.Publications;
using Application.Features.Publications.Dtos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PhotosController : BaseController
{
    private readonly ICloudinaryService _cloudinaryService;
    private readonly PublicationService _publicationService;

    public PhotosController(ICloudinaryService cloudinaryService, PublicationService publicationService)
    {
        _cloudinaryService = cloudinaryService;
        _publicationService = publicationService;
    }

    [HttpPost]
    public async Task<ActionResult<UploadResult>> Add([FromForm] IFormFile file)
    {
        var result = await _cloudinaryService.UploadImageAsync(file);
        return Ok(result);
    }

    [HttpDelete("{photoId}/{publicationId}")]
    public async Task<ActionResult> Delete(string photoId, Guid publicationId)
    {
        if (publicationId != Guid.Empty)
        {
            var publication = await _publicationService.GetPublicationById(publicationId);
            var photo = publication.Photos.FirstOrDefault(x => x.PhotoId == photoId);
            if (publication is not null && photo is not null)
            {
                var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var photos = publication.Photos.Where(x => x.PhotoId != photoId);
                
                if (photo.IsMain)
                {
                    var mainPhoto = photos.FirstOrDefault();

                    mainPhoto.IsMain = true;
                }


                await _publicationService.UpdatePublication(publication.Id, userId,
                    new UpdatePublicationRequest()
                        { Title = publication.Title, Content = publication.Content, Photos = photos.ToList() });
            }
        }

        await _cloudinaryService.DeleteImageAsync(photoId);
        return Ok();
    }
}