using Home.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Home
{
    public static class JwtTokenHelper
    {
        public static string GenerateToken(ApplicationUser user,string role, IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");

            var claims = new[]
            {  

            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, $"{user.FirstName??""} {user.LastName??""}"),
            new Claim("Photo",user.Photo ?? ""),
            new Claim("role",role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
            var kkkey = "";
            using (var rng = new RNGCryptoServiceProvider())
            {
                var secretKeyBytes = new byte[32]; // 256-bit key
                rng.GetBytes(secretKeyBytes);
                string secretKey = Convert.ToBase64String(secretKeyBytes);

                kkkey = secretKey;
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
