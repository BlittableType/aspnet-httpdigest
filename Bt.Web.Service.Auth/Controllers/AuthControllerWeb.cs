using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bt.Web.Service.Auth.Controllers
{
    [ApiController]
    [Route("v1")]
    public class AuthControllerWeb : ControllerBase
    {
        private readonly IAuthController _implementation;
    
        public AuthControllerWeb(IAuthController implementation)
        {
            _implementation = implementation;
        }
    
        /// <summary>Issues new JSON web token.</summary>
        /// <returns>Newly created JWT.</returns>
        [Authorize]
        [HttpGet, Route("token/new", Name = "newToken")]
        public Task<string> NewToken()
        {
            return _implementation.NewTokenAsync();
        }
    
        /// <summary>Checks if the server is running.</summary>
        /// <returns>Server is up and running.</returns>
        [HttpGet, Route("health", Name = "health")]
        public Task Health()
        {
            return _implementation.HealthAsync();
        }
    }
}