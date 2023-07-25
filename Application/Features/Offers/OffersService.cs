using System.Linq.Expressions;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Features.Offers.Requests;
using AutoMapper;
using Core.Entities;

namespace Application.Features.Offers;

public class OffersService
{
    private readonly IGenericRepository<Offer> _offersRepository;
    private readonly IMapper _mapper;

    public OffersService(IGenericRepository<Offer> offersRepository, IMapper mapper)
    {
        _offersRepository = offersRepository;
        _mapper = mapper;
    }

    public async Task<Guid> CreateOffer(Guid authorId, CreateOfferRequest request)
    {
        var offer = _mapper.Map<Offer>(request);
        offer.UserId = authorId;
        var id = await _offersRepository.AddAsync(offer);
        return id;
    }

    public async Task<Offer> GetOfferById(Guid id)
    {
        var includes = new List<Expression<Func<Offer, object>>>
        {
            p => p.User,
            p => p.Photos
        }.ToArray();
        return await _offersRepository.GetByIdAsync(id, includes);
    }


    public async Task<IReadOnlyList<Offer>> GetOffersByPublicationId(Guid publicationId)
    {
        var includes = new List<Expression<Func<Offer, object>>>
        {
            p => p.User,
            p => p.Photos
        }.ToArray();
        return await _offersRepository.ListAllAsync(x => x.PublicationId == publicationId, includes);
    }

    public async Task<IReadOnlyList<Offer>> GetOffersByUserIdForPublication(Guid userId, Guid publicationId)
    {
        var includes = new List<Expression<Func<Offer, object>>>
        {
            p => p.User,
            p => p.Photos
        }.ToArray();
        return await _offersRepository.ListAllAsync(x => x.UserId == userId && x.PublicationId == publicationId,
            includes);
    }

    public async Task RemoveOffer(Guid id, Guid userId)
    {
        var offer = await _offersRepository.GetByIdAsync(id);

        await _offersRepository.DeleteAsync(offer);
    }

    public async Task SetRank(IEnumerable<ReorderRequest> requests)
    {
        foreach (var request in requests)
        {
            var offer = await _offersRepository.GetByIdAsync(new Guid(request.OfferId));
            offer.Rank = request.Rank;
            await _offersRepository.UpdateAsync(offer);
        }
    }
}