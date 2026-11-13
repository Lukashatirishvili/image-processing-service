using System.Security.Claims;
using System.Text;
using ImageProcessingService.Domain.Entities;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace ImageProcessingService.Api.Controllers;

public sealed class JwtService(IConfiguration configuration)
{
    public string Create(AppUser user)
    {
        string? secretKey = configuration["jwt:Secretkey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("email verified", user.EmailConfirmed.ToString())
            ]),

            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("jwt:ExpirationTime")),
            SigningCredentials = credentials,
            Issuer = configuration["jwt:Issuer"],
            Audience = configuration["jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescription);

        return token;
    }
}