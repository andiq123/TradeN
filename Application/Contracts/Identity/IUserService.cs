using Application.Models;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Identity;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetUsers();
    Task<User> GetUserById(Guid id);
    Task CreateUser(Guid id, RegisterRequest user);
}