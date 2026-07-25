using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Services.Interfaces;
namespace Api.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [Authorize]
    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        var uploaded = await _fileService.UploadAsync(file, userId);

        return Ok(uploaded);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFiles()
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        var files = await _fileService.GetFilesAsync(userId);

        return Ok(files);
    }

    [Authorize]
    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromForm] IFormFile file)
    {
        // var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        // var uploaded = await _fileService.(file, userId);
        //ill leave it for later
        return Ok();
    }
}