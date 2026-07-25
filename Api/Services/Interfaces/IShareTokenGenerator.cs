using Api.Data.Models;

namespace Api.Services.Interfaces;

public interface IShareTokenGenerator
{
    public string Generate();
}