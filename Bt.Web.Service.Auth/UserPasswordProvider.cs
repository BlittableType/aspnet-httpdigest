using System.IO;
using System.Reflection;
using Bt.Web.Authentication.HttpDigest;
using Bt.Web.Authentication.HttpDigest.AspNetCore;
using Microsoft.Extensions.Configuration;

namespace Bt.Web.Service.Auth
{
    internal class UserPasswordProvider : IUserPasswordProvider
    {
        private readonly IConfiguration _configuration;

        public UserPasswordProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetPassword(string userName, string realm = null)
        {
            // User password file format is similar to Apache's .htdigest file.
            // Each row is of format: "userName:realm:password".

            var filePath = _configuration["HttpDigestSettings:UserPasswordFilePath"];
            
            filePath = File.Exists(filePath) 
                ? filePath
                : Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "UserAccess", "user-access.txt");
            
            string[] fileRecord;

            using (var reader = File.OpenText(filePath))
            {
                while (!reader.EndOfStream)
                {
                    fileRecord = reader.ReadLine().Split(':');

                    if (fileRecord[0] == userName && (realm == null || fileRecord[1] == realm))
                        return fileRecord[2];
                }
            }

            return null;
        }
    }
}