


using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core
{
    public class JwtHelper : ITokenHelper
    {
        
        public JwtHelper()
        {
           
        }

        public string GenerateToken(string username, string password)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Email,username)
            };
            string signinkey = "BuBenimSigningKeyBuBenimSigningKey";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinkey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "admin.admin@gmail.com",
                audience: "BuBenimKullabdıgımAudienceDegeri",
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                notBefore: DateTime.Now,
                signingCredentials: credentials
                );


            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }


    }
}
