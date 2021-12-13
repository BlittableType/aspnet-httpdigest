using System.Threading.Tasks;

namespace Bt.Web.Service.Auth.Controllers
{
    public interface IAuthController
    {
        /// <summary>Issues new JSON web token.</summary>
        /// <returns>Newly created JWT.</returns>
        Task<string> NewTokenAsync();
    
        /// <summary>Checks if the server is running.</summary>
        /// <returns>Server is up and running.</returns>
        Task HealthAsync();
    }
}