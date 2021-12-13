using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bt.Web.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Bt.Web.Authentication.HttpDigest.AspNetCore
{
    public class HttpDigestHandler : AuthenticationHandler<HttpDigestOptions>
    {
        private readonly IUserPasswordProvider _userPasswordProvider;
        private readonly INonceStorage _nonceStorage;

        public HttpDigestHandler(IOptionsMonitor<HttpDigestOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IUserPasswordProvider userPasswordProvider,
            INonceStorage nonceStorage) :
            base(options, logger, encoder, clock)
        {
            _userPasswordProvider = userPasswordProvider;
            _nonceStorage = nonceStorage;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorization = Request.Headers[HeaderNames.Authorization];

            // If no authorization header found, nothing to process further
            if (string.IsNullOrEmpty(authorization))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            string digestAuthRequest = String.Empty;

            if (authorization.StartsWith("Digest ", StringComparison.OrdinalIgnoreCase))
            {
                digestAuthRequest = authorization.Substring("Digest ".Length).Trim();
            }

            // If client request is empty, no further work possible
            if (string.IsNullOrEmpty(digestAuthRequest))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // Parse client request

            var stringValues = digestAuthRequest.Split(',');
            var values = new Dictionary<string, string>();

            foreach (var s in stringValues)
            {
                var keyValue = s.Split('=');
                values[keyValue[0].TrimStart()] = keyValue[1].Trim('"');
            }

            var userName = values["username"];
            ulong nonceCount = UInt64.Parse(values["nc"].TrimStart('0'));
            
            // Check nonce value
            
            if (!ProcessNonce(values["nonce"], nonceCount))
                return Task.FromResult<AuthenticateResult>(AuthenticateResult.Fail("digest.auth.nonce.not.valid"));

            // Generate response
            
            var response = HttpDigestResponseBuilder.Build(
                Options.HashingAlgorithm,
                userName: userName,
                realm: values["realm"],
                _userPasswordProvider.GetPassword(userName, Options.Realm),
                nonce: values["nonce"],
                httpMethod: new HttpMethod(Request.Method),
                uri: new Uri(values["uri"], UriKind.Relative),
                qop: Qop.Parse(values["qop"]),
                nonceCount: nonceCount,
                cnonce: values["cnonce"]);

            var identity = new ClaimsIdentity(
                authenticationType: "AuthenticationTypes.Federation",
                nameType: ClaimsIdentity.DefaultNameClaimType,
                roleType: ClaimsIdentity.DefaultRoleClaimType);

            return response == values["response"]
                ? Task.FromResult<AuthenticateResult>(
                    AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)))
                : Task.FromResult<AuthenticateResult>(AuthenticateResult.Fail("digest.auth.wrong.response"));
        }

        private bool ProcessNonce(string nonceString, ulong nonceCount)
        {
            byte[] nonceBytes = nonceString.ToByteArray();
            ulong nonceVal = BitConverter.ToUInt64(nonceBytes);
            var nonce = _nonceStorage.Get(nonceVal);

            // Check if exists
            
            if (nonce == null)
                return false;

            // Check if expired
            
            if (DateTimeOffset.UtcNow - nonce.Created >= Options.NonceTtl)
            {
                _nonceStorage.Remove(nonce);
                return false;
            }
            
            // Check if nonceCount is OK

            nonce.IncrementCount();

            return nonceCount == nonce.Count;
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            
            var nonce = new Nonce();
            _nonceStorage.Save(nonce);

            var wwwAuthenticate = new StringBuilder();

            wwwAuthenticate.Append($"realm=\"{Options.Realm}\",");
            wwwAuthenticate.Append($"qop=\"{Qop.Auth},{Qop.AuthInt}\",");
            wwwAuthenticate.Append($"nonce=\"{BitConverter.GetBytes(nonce.Value).ToHexString().ToLower()}\"");

            Response.Headers.Append(HeaderNames.WWWAuthenticate, wwwAuthenticate.ToString());

            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return Task.CompletedTask;
        }
    }
}