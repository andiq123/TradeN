using Application.Contracts.Cloudinary;
using Application.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using UploadResult = Infrastructure.Services.Models.UploadResult;

namespace Infrastructure.Services;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var cloudinarySettings = config.Value;
        var acc = new Account(
            cloudinarySettings.Cloud,
            cloudinarySettings.ApiKey,
            cloudinarySettings.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<UploadResult> UploadImageAsync(IFormFile file)
    {
        if (file.Length <= 0) throw new BadRequestException("Invalid file");

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return new UploadResult()
            { Id = Guid.NewGuid(), PhotoId = uploadResult.PublicId, Url = uploadResult.SecureUrl.AbsoluteUri };
    }
    
    public async Task<UploadResult> UploadBytesAsync(string name, Stream stream)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(name, stream),
            Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        return new UploadResult()
            { Id = Guid.NewGuid(), PhotoId = uploadResult.PublicId, Url = uploadResult.SecureUrl.AbsoluteUri };
    }

    public async Task DeleteImageAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }

    public async Task DeleteAllImagesAsync()
    {
        await _cloudinary.DeleteAllResourcesAsync();
    }
}