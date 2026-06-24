using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookReadingEvents.Application.Interfaces;
using BookReadingEvents.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookReadingEvents.Infrastructure.JWT;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings) // used IOptions <T> to inject the JwtSettings from the configuration file (appsettings.json) into the JwtService class. This allows the service to access the JWT settings, such as the secret key, issuer, audience, and expiry time, which are necessary for generating JWT tokens.
    {
        _jwtSettings = jwtSettings.Value;
    }

    public Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim> // claim is nothing but fact about the User.
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));  // convert the secret key from the configuration file into a byte array and create a SymmetricSecurityKey object. This key will be used to sign the JWT token, ensuring its integrity and authenticity.

        var credentials =
            new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // create a SigningCredentials object using the symmetric key and the HMAC-SHA256 algorithm. This object will be used to sign the JWT token, ensuring that it cannot be tampered with.

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return Task.FromResult(
            new JwtSecurityTokenHandler().WriteToken(token));
    }
}