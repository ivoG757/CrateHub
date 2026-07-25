using System.Security.Cryptography;
using Api.Services.Interfaces;
namespace Api.Services;

public class ShareTokenGenerator : IShareTokenGenerator
{

    public string Generate()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }
}