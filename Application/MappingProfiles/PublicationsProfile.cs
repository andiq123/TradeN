using Application.Features.Publications.Dtos;
using AutoMapper;
using Core.Entities;

namespace Application.MappingProfiles;

public class PublicationsProfile : Profile
{
    public PublicationsProfile()
    {
        CreateMap<CreatePublicationRequest, Publication>()
            .ForMember(x => x.PublishDate,
                opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(x=>x.IsAvailable, opt => opt.MapFrom(src => true));
    }
}