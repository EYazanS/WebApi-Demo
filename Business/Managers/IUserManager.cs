using Business.Resources;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Managers
{
    public interface IUserManager
    {
        string Authenticate(AuthenticateResource model);
    }

    public class UserManager : IUserManager
    {
        private readonly IConfiguration _config;

        public UserManager(IConfiguration config)
        {
            _config = config;
        }

        public string Authenticate(AuthenticateResource model)
        {
            if (model.Username == "guest" && model.Password == "guest")
            {
               return GenerateJWTToken(model);
            }

            return null;
        }


        string GenerateJWTToken(AuthenticateResource model)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["SecretKey"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Issuer"],
                audience: _config["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

