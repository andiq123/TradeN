using System.Linq.Expressions;
using Application.Contracts.Cloudinary;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.Publications.Dtos;
using AutoMapper;
using Core.Entities;
using Infrastructure.Services;

namespace Application.Features.Publications;

public class PublicationService
{
    private readonly IGenericRepository<Publication> _publicationRepository;
    private readonly IGenericRepository<Photo> _photoRepository;
    private readonly ICloudinaryService _cloudinaryService;
    private readonly IMapper _mapper;

    public PublicationService(IGenericRepository<Publication> publicationRepository,
        IGenericRepository<Photo> photoRepository,
        ICloudinaryService cloudinaryService,
        IMapper mapper)
    {
        _publicationRepository = publicationRepository;
        _photoRepository = photoRepository;
        _cloudinaryService = cloudinaryService;
        _mapper = mapper;
    }

    public async Task<Publication> GetPublicationById(Guid id)
    {
        var includes = new List<Expression<Func<Publication, object>>>
        {
            p => p.User,
            p => p.Photos
        }.ToArray();
        return await _publicationRepository.GetByIdAsync(id, includes);
    }

    //set publication not available
    public async Task SetPublicationNotAvailable(Guid id)
    {
        var publication = await _publicationRepository.GetByIdAsync(id);

        publication.IsAvailable = false;
        await _publicationRepository.UpdateAsync(publication);
    }

    public async Task<IReadOnlyList<Publication>> GetPublications(ListPublicationsRequest request)
    {
        var includes = new List<Expression<Func<Publication, object>>>
        {
            p => p.User,
            p => p.Photos
        }.ToArray();

        //add default orderby by isAvaiable
        var orders = new List<Expression<Func<Publication, object>>>
        {
            p => p.IsAvailable,
            p => p.PublishDate
        }.ToArray();


        var filter = request.Title is null || request.Title == ""
            ? (Expression<Func<Publication, bool>>)null
            : x => x.Title.ToLower().Contains(request.Title.ToLower());


        return await _publicationRepository.ListAllAsync(filter, includes, orders, request.Descending);
    }

    public async Task<IReadOnlyList<Publication>> GetPublications(Guid userId)
    {
        var includes = new List<Expression<Func<Publication, object>>>
        {
            p => p.User,
            p => p.Photos
        }.ToArray();
        return await _publicationRepository.ListAllAsync(x => x.UserId == userId, includes);
    }

    public async Task<Guid> CreatePublication(Guid authorId, CreatePublicationRequest request)
    {
        if (request.Photos.Count() == 1)
        {
            request.Photos.First().IsMain = true;
        }

        var publication = _mapper.Map<Publication>(request);
        publication.UserId = authorId;
        var id = await _publicationRepository.AddAsync(publication);
        return id;
    }

    public async Task UpdatePublication(Guid id, Guid userId, UpdatePublicationRequest request)
    {
        if (request.Photos is { Count: 1 })
        {
            request.Photos.First().IsMain = true;
        }

        var includes = new List<Expression<Func<Publication, object>>>
        {
            p => p.Photos!
        }.ToArray();

        var publication =
            await _publicationRepository.GetByIdAsync(id);

        publication.Title = request.Title;
        publication.Content = request.Content;
        publication.Photos = request.Photos;

        await _publicationRepository.UpdateAsync(publication);
    }

    public async Task RemovePublication(Guid id, Guid userId)
    {
        var includes = new List<Expression<Func<Publication, object>>>
        {
            p => p.Photos!
        }.ToArray();

        var publication =
            await _publicationRepository.GetByIdAsync(id,
                includes);

        if (publication.UserId != userId)
            throw new NotFoundException(nameof(Publication), id);

        if (publication.Photos != null && publication.Photos.Any())
        {
            var photos = publication.Photos.ToList();
            foreach (var photo in photos)
            {
                await _photoRepository.DeleteAsync(photo);
                await _cloudinaryService.DeleteImageAsync(photo.PhotoId);
            }
        }

        await _publicationRepository.DeleteAsync(publication);
    }
}