using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActividadApp.Models;
using Microsoft.IdentityModel.Tokens;

namespace ActividadApp.Services;

public class JwtService
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtService(IConfiguration configuration)
    {
        _key = Environment.GetEnvironmentVariable("JWT_KEY")
            ?? configuration["Jwt:Key"]!;
        _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? configuration["Jwt:Issuer"]!;
        _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
            ?? configuration["Jwt:Audience"]!;
    }

    public string GenerateToken(Usuario user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.RolId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public UserSession? GetUserSessionFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null)
            return null;

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
        var usernameClaim = principal.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(emailClaim))
            return null;

        return new UserSession
        {
            UserId = int.Parse(userIdClaim),
            Email = emailClaim,
            Username = usernameClaim ?? string.Empty,
            IsAuthenticated = true
        };
    }
}
