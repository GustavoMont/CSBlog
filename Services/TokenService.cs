using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSBlog.Models;
using Microsoft.IdentityModel.Tokens;

namespace CSBlog.Services;

public class TokenService
{
    private string jwtkey = System.Environment.GetEnvironmentVariable("JWT_KEY");
    private string audience = System.Environment.GetEnvironmentVariable("AUDIENCE");

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtkey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Role, user.UserType.ToString()),
                    new Claim("id", user.Id.ToString())
                }
            ),
            Audience = "jhonsonkkkkkk",
            Expires = DateTime.Now.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateResetPasswordToken(int id)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtkey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", id.ToString()) }),
            Expires = DateTime.Now.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Audience = audience
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool IsTokenValid(string jwt)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                // Configurações de validação
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtkey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudiences = new[] { audience },
                ValidateIssuer = false,
            };
            handler.ValidateToken(jwt, validationParameters, out SecurityToken token);
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public int GetUserId(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        string userId = jsonToken.Claims.First(claim => claim.Type == "id")?.Value;
        return int.Parse(userId);
    }
}
