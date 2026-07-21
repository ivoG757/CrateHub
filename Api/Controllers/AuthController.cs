using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Api.Data.Dtos;
using Api.Services.Interfaces;
namespace Api.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService service)
    {
        _authService = service;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenDto dto)
    {
        var tokens = await _authService.RefreshAsync(dto);

        return Ok(tokens);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var tokens = await _authService.RegisterAsync(dto);
        return Ok(tokens);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var tokens = await _authService.LoginAsync(dto);
        return Ok(tokens);
    }
}