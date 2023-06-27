using Application.Features.Exchanges.Requests;
using AutoMapper;
using Core.Entities;

namespace Application.MappingProfiles;

public class ExchangeProfile : Profile
{
    public ExchangeProfile()
    {
        CreateMap<CreateExchangeRequest, Exchange>()
            .ForMember(x => x.ExchangeDate, opt => opt.MapFrom(src => DateTime.Now));
    }
}