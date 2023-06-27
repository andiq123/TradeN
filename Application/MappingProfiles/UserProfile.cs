using Application.Models;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<IdentityUser, User>();
        CreateMap<RegisterRequest, User>();
    }
}