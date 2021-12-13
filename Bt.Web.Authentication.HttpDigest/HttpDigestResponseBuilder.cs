using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Bt.Web.Common;

namespace Bt.Web.Authentication.HttpDigest
{
    public static class HttpDigestResponseBuilder
    {
        public static string Build(
            HashingAlgorithm hashingAlgorithm,
            string userName,
            string realm, 
            string password,
            string nonce, 
            HttpMethod httpMethod, 
            Uri uri, 
            Qop qop,
            ulong nonceCount,
            string cnonce)
        {
            var ha1 = ComputeHash(hashingAlgorithm, $"{userName}:{realm}:{password}");
            var ha2 = ComputeHash(hashingAlgorithm, $"{httpMethod}:{uri}");
            var responseString = $"{ha1}:{nonce}";

            if (qop == Qop.Auth || qop == Qop.AuthInt)
                responseString += $":{nonceCount:00000000}:{cnonce}:{qop}";

            responseString += $":{ha2}";

            return ComputeHash(hashingAlgorithm, responseString);
        }

        private static string ComputeHash(HashingAlgorithm hashingAlgorithm, string message)
        {
            var msgBytes = Encoding.ASCII.GetBytes(message);
            HashAlgorithm hashAlg;
            
            switch (hashingAlgorithm)
            {
                case HashingAlgorithm.Md5:
                    hashAlg = MD5.Create();
                    break;
                default:
                    hashAlg = SHA256Managed.Create();
                    break;
            }
            
            return hashAlg.ComputeHash(msgBytes).ToHexString().ToLowerInvariant();
        }
    }
}