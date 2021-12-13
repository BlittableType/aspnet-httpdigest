using System;
using Microsoft.AspNetCore.Authentication;

namespace Bt.Web.Authentication.HttpDigest.AspNetCore
{
    public class HttpDigestOptions : AuthenticationSchemeOptions
    {
        public HashingAlgorithm HashingAlgorithm { get; set; } = HashingAlgorithm.Sha256;
        public string Realm { get; set; }
        public TimeSpan NonceTtl { get; set; }
    }
}