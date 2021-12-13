using System;
using Microsoft.AspNetCore.Authentication;

namespace Bt.Web.Authentication.HttpDigest.AspNetCore
{
    public static class HttpDigestExtensions
    {
        public static AuthenticationBuilder AddHttpDigest(this AuthenticationBuilder builder, string authenticationScheme, Action<HttpDigestOptions> configureOptions)
            => builder.AddScheme<HttpDigestOptions, HttpDigestHandler>(authenticationScheme, null, configureOptions);
    }
}