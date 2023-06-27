
using Infrastructure.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public interface ICloudinaryService
{
    Task<UploadResult> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string publicId);
}