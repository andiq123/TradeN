using Infrastructure.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Cloudinary;

public interface ICloudinaryService
{
    Task<UploadResult> UploadImageAsync(IFormFile file);
    Task DeleteImageAsync(string publicId);
    Task DeleteAllImagesAsync();
    Task<UploadResult> UploadBytesAsync(string name, Stream stream);
}