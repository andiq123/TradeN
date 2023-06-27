using System.Linq.Expressions;
using Application.Contracts.Persistence;
using Application.Features.Exchanges.Requests;
using AutoMapper;
using Core.Entities;

namespace Application.Features.Exchanges;

public class ExchangeService
{
    private readonly IGenericRepository<Exchange> _exchangeRepository;
    private readonly IMapper _mapper;

    public ExchangeService(IGenericRepository<Exchange> exchangeRepository, IMapper mapper)
    {
        _exchangeRepository = exchangeRepository;
        _mapper = mapper;
    }

    public async Task<Exchange> CreateExchangeAsync(CreateExchangeRequest request)
    {
        var exchange = _mapper.Map<Exchange>(request);
        var id = await _exchangeRepository.AddAsync(exchange);
        return await _exchangeRepository.GetByIdAsync(id);
    }

    public async Task<Exchange> GetExchangeByIdAsync(Guid id)
    {
        var includes = new List<Expression<Func<Exchange, object>>>()
        {
            x => x.Publication,
            x => x.Author,
            x => x.OfferUser,
            x => x.Offer,
        }.ToArray();
        return await _exchangeRepository.GetByIdAsync(id, includes);
    }

    public async Task<IEnumerable<Exchange>> GetMyExchangesAsync(Guid use)
    {
        var includes = new List<Expression<Func<Exchange, object>>>()
        {
            x => x.Publication,
            x => x.Author,
            x => x.OfferUser,
            x => x.Offer,
        }.ToArray();
        return await _exchangeRepository.ListAllAsync(x => x.AuthorId == use || x.OfferUserId == use, includes);
    }
}