using Api.Services.Interfaces;
using Api.Data.Dtos;
using Api.Repository.Interfaces;
using Api.Data.Models;
using Microsoft.AspNetCore.Identity;
using Api.Exceptions;
using Api.Repositories;
namespace Api.Services;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher<User> _hasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRep;
    private readonly IRefreshTokenRepository _refTokenRepository;
    private readonly ILogger<AuthService> _logger;
    public AuthService(IUserRepository userRep,
     IRefreshTokenRepository tokenRepository,
      ITokenService tokenService,
       IPasswordHasher<User> hasher,
        IUnitOfWork unitOfWOrk,
        ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWOrk;
        _tokenService = tokenService;
        _userRep = userRep;
        _hasher = hasher;
        _refTokenRepository = tokenRepository;
        _logger = logger;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRep.GetUserByNameAsync(dto.Username);

        if (user == null)
        {
            _logger.LogWarning(
                "Failed login attempt for username {Username}: user not found",
                dto.Username);
            throw new InvalidCredentialsException();
        }

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning(
                "Failed login attempt: invalid password");
            throw new InvalidCredentialsException();
        }
        var response = await TokenCycleResponseAsync(user);

        _logger.LogInformation("User {Username} with id: {Id} logged in successfully", user.Username, user.Id);

        return response;
    }

    public async Task<AuthResponseDto> RefreshAsync(RefreshTokenDto dto)
    {
        var token = await _refTokenRepository.GetByTokenAsync(dto.Token);

        if (token is null)
        {
            _logger.LogWarning(
                "Refresh token rotation failed: token was invalid or not found");
            throw new InvalidRefreshTokenException();
        }

        if (token.ExpiresAt < DateTime.UtcNow)
        {
            _refTokenRepository.Delete(token);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogWarning("Refresh token rotation failed: token was expired.");

            throw new InvalidRefreshTokenException(); //TODO: think of what to do with InvalidRefreshTokenException
        }
        var user = token?.User;

        if (user is null)
        {
            _logger.LogWarning("Refresh token rotation failed: user does not exist.");
            throw new InvalidRefreshTokenException();
        }
        _refTokenRepository.Delete(token!);
        // Delete the old refresh token to prevent reuse (refresh token rotation)

        var response = await TokenCycleResponseAsync(user);


        _logger.LogInformation(
            "Refresh token rotated for user {Username} with id: {Id}",
             user.Username, user.Id);

        return response;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (await _userRep.UserWithEmailExistsAsync(dto.Email))
        {
            _logger.LogWarning(
                "Registration failed: email {Email} already exists",
                dto.Email);

            throw new EmailAlreadyExistsException();
        }

        if (await _userRep.UserWithNameExistsAsync(dto.Username))
        {
            _logger.LogWarning(
               "Registration failed: username {Username} already exists",
               dto.Username);
            throw new UsernameAlreadyExistsException();
        }

        var user = new User();

        var hash = _hasher.HashPassword(user, dto.Password);

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.PasswordHash = hash;

        var savedUser = await _userRep.AddAsync(user);

        var response = await TokenCycleResponseAsync(savedUser);

        _logger.LogInformation(
            "User registered successfully with id {UserId}",
            savedUser.Id);

        return response;
    }
    private async Task<AuthResponseDto> TokenCycleResponseAsync(User user)
    {
        await _unitOfWork.SaveChangesAsync();
        // to ensure that any changes to the user or refresh tokens are persisted before creating the auth response

        _logger.LogDebug(
        "Creating authentication response for user {UserId}",
        user.Id);


        var accessToken = _tokenService.CreateToken(user.Id, user.Username);

        var refreshToken = _tokenService.CreateRefreshToken();

        await RemoveExpiredTokensAsync(user);
        // Remove expired tokens before adding a new one to prevent token accumulation 
        // or atleast to keep the number of tokens manageable...

        await _refTokenRepository.AddAsync(new RefreshToken
        {
            User = user,
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
    private async Task RemoveExpiredTokensAsync(User user)
    {
        var expiredTokens = await _userRep.GetExpiredUsersTokens(user.Id);

        foreach (var token in expiredTokens)
        {
            _refTokenRepository.Delete(token);
        }
    }
}