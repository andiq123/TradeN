using System.Text.Json;
using Application.Contracts.Cloudinary;
using Application.Contracts.Identity;
using Application.Features.Offers;
using Application.Features.Offers.Requests;
using Application.Features.Publications;
using Application.Features.Publications.Dtos;
using Application.Models;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Seed;

public class Seeder
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        //services
        var authService = serviceProvider.GetRequiredService<IAuthService>();
        var publicationService = serviceProvider.GetRequiredService<PublicationService>();
        var offersService = serviceProvider.GetRequiredService<OffersService>();
        var cloudinaryService = serviceProvider.GetRequiredService<ICloudinaryService>();
        var tradeNContext = serviceProvider.GetRequiredService<TradeNContext>();
        
        var usersExists = await tradeNContext.Users.AnyAsync();
        if (usersExists)
            return;

        //photos 
        var photos = new string[] { "cameraNikon", "bicicletaDeMunte", "playstation5", "iphone12pro", "dellxp15" };
        var photosFullName = photos.Select(x => "Seed/Photos/" + x + ".jpg").ToArray();

        var photosBytes = photosFullName.Select(File.ReadAllBytes);
        var photoStreams = photosBytes.Select(x => new MemoryStream(x)).ToArray();

        await cloudinaryService.DeleteAllImagesAsync();

        var uploadResults = new List<UploadResult>();
        for (var i = 0; i < photos.Length; i++)
        {
            var uploadResult = await cloudinaryService.UploadBytesAsync(photosFullName[i], photoStreams[i]);
            uploadResults.Add(uploadResult);
        }


        //users
        var jsonUsersText = await File.ReadAllTextAsync("Seed/UsersSeed.json");
        var users = JsonSerializer.Deserialize<List<RegisterRequest>>(jsonUsersText);
        var userIds = users!
            .Select(async x => await authService.RegisterAsync(x))
            .Select(x => new Guid(x.Result.Id)).ToArray();


        //publications
        var jsonPublicationsText = await File.ReadAllTextAsync("Seed/PublicationsSeed.json");
        var publications = JsonSerializer.Deserialize<List<CreatePublicationRequest>>(jsonPublicationsText);

        var publicationIds = new List<Guid>();
        for (var i = 0; i < publications!.Count; i++)
        {
            var publication = publications![i];
            var randomId = new Random().Next(0, userIds.Length);
            var userId = userIds[randomId];
            publication.Photos = new List<Photo>()
            {
                new Photo()
                {
                    PhotoId = uploadResults[i].PhotoId, Id = Guid.NewGuid(), Url = uploadResults[i].Url, IsMain = true
                }
            };
            var id = await publicationService.CreatePublication(userId, publication);
            publicationIds.Add(id);
        }

        var publicationId = publicationIds[0].ToString();

        publicationIds = null;

        var publicationObject = await publicationService.GetPublicationById(new Guid(publicationId));

        //offers
        var jsonOffersText = await File.ReadAllTextAsync("Seed/OffersSeed.json");
        var offers = JsonSerializer.Deserialize<List<CreateOfferRequest>>(jsonOffersText);

        for (var i = 0; i < offers!.Count; i++)
        {
            var offer = offers![i];
            offer.PublicationId = publicationId;

            var randomId = new Random().Next(0, userIds.Length);
            var userId = userIds[randomId];

            while (userId == publicationObject.UserId)
            {
                randomId = new Random().Next(0, userIds.Length);
                userId = userIds[randomId];
            }


            await offersService.CreateOffer(userId, offer);
        }
    }
}