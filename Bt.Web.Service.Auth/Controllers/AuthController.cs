using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Bt.Web.Service.Auth.Controllers
{
    public class AuthController : IAuthController
    {
        private readonly IConfiguration _configuration;
        
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        /// <summary>Issues new JSON web token.</summary>
        /// <returns>Newly created JWT.</returns>
        public Task<string> NewTokenAsync()
        {
            var keyPath = _configuration["JwtSettings:SigningKeyPath"];

            keyPath = !File.Exists(keyPath)
                ? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Keys", "priv-key.pem")
                : keyPath;
            
            var key = ECDsa.Create();
            key.ImportFromPem(File.ReadAllText(keyPath));
            
            var signCredentials = new SigningCredentials(new ECDsaSecurityKey(key), _configuration["JwtSettings:SigningAlgorithm"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenExpiresMinutes = Double.Parse(_configuration["JwtSettings:TokenExpiresMinutes"]);
            
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor()
            {
                SigningCredentials = signCredentials,
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                IssuedAt =  DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiresMinutes)
            });

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        /// <summary>Checks if the server is running.</summary>
        /// <returns>Server is up and running.</returns>
        public Task HealthAsync()
        {
            return Task.CompletedTask;
        }
    }
}