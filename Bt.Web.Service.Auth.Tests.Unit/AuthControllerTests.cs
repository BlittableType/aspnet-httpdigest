using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Bt.Web.Service.Auth.Controllers;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace Bt.Web.Service.Auth.Tests.Unit
{
    public class AuthControllerTests
    {
        [Test]
        public async Task NewTokenAsync_should_create_valid_JWT()
        {
            // Given
            var controller = new AuthController(new ConfigurationMock(new Dictionary<string, string>()
            {
                { "JwtSettings:SigningKeyPath", "priv-key.pem" },
                { "JwtSettings:SigningAlgorithm", "ES512" },
                { "JwtSettings:TokenExpiresMinutes", "60" },
                { "JwtSettings:Issuer", "Bt.Web.Service.Auth" },
                { "JwtSettings:Audience", "Bt.Web.Service" }
            }));
            
            // When
            var jwt = await controller.NewTokenAsync();

            // Then

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = null;
            
            Assert.That(() => tokenHandler.ValidateToken(jwt, new TokenValidationParameters()
            {
                ValidIssuer = "Bt.Web.Service.Auth",
                ValidAudience = "Bt.Web.Service",
                IssuerSigningKey = GetSigningKey()
            }, out token), Throws.Nothing);
            
            Assert.That(token?.Issuer, Is.EqualTo("Bt.Web.Service.Auth"));
            Assert.That(token.ValidTo, Is.LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(60)));
        }

        private SecurityKey GetSigningKey()
        {
            var key = ECDsa.Create();
            key.ImportFromPem(File.ReadAllText("pub-key.pem"));
            return new ECDsaSecurityKey(key);
        }
    }
}