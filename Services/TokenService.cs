using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CSBlog.Models;
using Microsoft.IdentityModel.Tokens;

namespace CSBlog.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        string jwtkey = System.Environment.GetEnvironmentVariable("jwtkey");
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
            Expires = DateTime.Now.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
