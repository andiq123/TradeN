using Application.Contracts.Identity;
using Application.Contracts.Persistence;
using Application.Exceptions;
using Application.Models;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TradeNIdentity.cs.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IMapper _mapper;


    public UserService(IGenericRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<User>> GetUsers()
    {
        return await _userRepository.ListAllAsync();
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task CreateUser(Guid id, RegisterRequest request)
    {
        var userToCreate = _mapper.Map<User>(request);
        userToCreate.Id = id;
        await _userRepository.AddAsync(userToCreate);
    }
}