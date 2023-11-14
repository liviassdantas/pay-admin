using AuthService.Model;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using AuthService.DTO;
using AuthService.Interfaces;
using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Service
{
    public class AuthServices : IAuthService
    {
        public AuthServices() { }

        public async Task<string> GenerateToken(UserDTO user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretConfig = new Settings().Secret;
                var key = Encoding.ASCII.GetBytes(secretConfig);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Email.ToString()),
                        new Claim(ClaimTypes.Email, user.Email.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(6),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);

            }
            catch (Exception)
            {
                throw new Exception("Can't login, please, try again another time");
            }
        }

    }
}