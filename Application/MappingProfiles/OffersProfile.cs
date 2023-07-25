using Application.Features.Offers.Requests;
using AutoMapper;
using Core.Entities;

namespace Application.MappingProfiles;

public class OffersProfile : Profile
{
    public OffersProfile()
    {
        CreateMap<CreateOfferRequest, Offer>()
            .ForMember(x => x.OfferDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(x => x.PublicationId, opt => opt.MapFrom(src => Guid.Parse(src.PublicationId)))
            .ForMember(x => x.Rank, opt => opt.MapFrom(src => 0));
    }
}