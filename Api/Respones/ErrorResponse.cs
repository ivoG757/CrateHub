namespace Api.Responses;

public sealed class ErrorResponse
{
    public string Code { get; init; } = null!;
    public string Message { get; init; } = null!;
    public Dictionary<string, string[]>? Errors { get; init; }
}