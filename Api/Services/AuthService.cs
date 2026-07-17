using Api.Services.Interaces;
using Api.Data.Dtos;
using Api.Repository.Interfaces;
using Api.Data.Models;
using Microsoft.AspNetCore.Identity;
using Api.Exceptions;
using Api.Constants;
namespace Api.Services;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher<User> _hasher;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRep;
    public AuthService(IUserRepository rep, ITokenService tokenService, IPasswordHasher<User> hasher)
    {
        _tokenService = tokenService;
        _userRep = rep;
        _hasher = hasher;
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userRep.GetUserByNameAsync(dto.Username);

        if (user == null)
        {
            throw new InvalidCredentialsException();
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            throw new InvalidCredentialsException();
        }

        return _tokenService.CreateToken(user.Id, user.Username);
    }

    public async Task<string> RegisterAsync(RegisterDto dto)
    {
        if (await _userRep.UserWithEmailExistsAsync(dto.Email))
        {
            throw new EmailAlreadyExistsException();
        }

        if (await _userRep.UserWithNameExistsAsync(dto.Username))
        {
            throw new UsernameAlreadyExistsException();
        }

        var user = new User();

        var hash = _hasher.HashPassword(user, dto.Password);

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.PasswordHash = hash;

        var savedUser = await _userRep.SaveUserAsync(user);

        return _tokenService.CreateToken(savedUser.Id, savedUser.Username);
    }
}