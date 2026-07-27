using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Api.Services.Interfaces;
using Api.Data.Dtos;
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
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var userId = GetUserId();

        var uploaded = await _fileService.UploadAsync(file, userId);

        return Ok(uploaded);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFiles()
    {
        var userId = GetUserId();

        var files = await _fileService.GetFilesAsync(userId);

        return Ok(files);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        await _fileService.DeleteAsync(id, userId);

        return NoContent();
    }
    public int GetUserId()
    {
        return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    }
}