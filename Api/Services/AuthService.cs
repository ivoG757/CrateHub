using Api.Services.Interfaces;
using Api.Data.Dtos;
using Api.Repository.Interfaces;
using Api.Data.Models;
using Microsoft.AspNetCore.Identity;
using Api.Exceptions;
using Api.Constants;
using Api.Responses;
namespace Api.Services;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher<User> _hasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRep;
    private readonly IRefreshTokenRepository _refTokenRepository;
    public AuthService(IUserRepository userRep, IRefreshTokenRepository tokenRepository, ITokenService tokenService, IPasswordHasher<User> hasher, IUnitOfWork unitOfWOrk)
    {
        _unitOfWork = unitOfWOrk;
        _tokenService = tokenService;
        _userRep = userRep;
        _hasher = hasher;
        _refTokenRepository = tokenRepository;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
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

        return await CreateAuthResponseAsync(user);
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshTokenDto dto)
    {
        var token = await _refTokenRepository.GetByTokenAsync(dto.Token);


        if (token is null)
        {
            throw new InvalidRefreshTokenException();
        }
        if (token.ExpiresAt < DateTime.UtcNow)
        {
            _refTokenRepository.Delete(token);
            await _unitOfWork.SaveChangesAsync();
            throw new InvalidRefreshTokenException();
        }

        var user = token?.User;

        if (user is null)
        {
            throw new InvalidRefreshTokenException();
        }

        var newRefreshToken = _tokenService.CreateRefreshToken();

        _refTokenRepository.Delete(token);

        await _refTokenRepository.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });
        await _unitOfWork.SaveChangesAsync();

        var accessToken = _tokenService.CreateToken(user.Id, user.Username);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
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

        var savedUser = await _userRep.AddAsync(user);

        return await CreateAuthResponseAsync(savedUser);
    }
    private async Task<AuthResponseDto> CreateAuthResponseAsync(User user)
    {
        var accessToken = _tokenService.CreateToken(user.Id, user.Username);

        var refreshToken = _tokenService.CreateRefreshToken();

        foreach (var token in user.RefreshTokens.Where(t => t.ExpiresAt < DateTime.UtcNow))
        {
            _refTokenRepository.Delete(token);
        }

        await _refTokenRepository.AddAsync(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}