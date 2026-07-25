using Api.Services.Interfaces;

namespace Api.Services;

public class UrlProvider : IUrlProvider
{
    private readonly string _url;
    public UrlProvider(IConfiguration configuration)
    {
        _url = configuration["AppSettings:BaseUrl"]
        ?? throw new Exception("Base URL is missing");
    }

    public string Create(string token)
    {
        return $"{_url}/api/files/download/{token}";
    }
}