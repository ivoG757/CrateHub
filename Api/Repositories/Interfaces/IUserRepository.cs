using Microsoft.AspNetCore.Identity;
using Api.Data.Models;
namespace Api.Repository.Interfaces;

public interface IUserRepository
{
    public Task<bool> UserWithNameExistsAsync(string name);
    public Task<bool> UserWithEmailExistsAsync(string email);
    public Task<User?> GetUserByIdAsync(int id);
    public Task<User?> GetUserByNameAsync(string name);
    public Task<User> AddAsync(User user);
}