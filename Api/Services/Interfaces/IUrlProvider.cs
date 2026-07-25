using Api.Data.Dtos;

namespace Api.Services.Interfaces;

public interface IUrlProvider
{
    public string Create(string token);
}