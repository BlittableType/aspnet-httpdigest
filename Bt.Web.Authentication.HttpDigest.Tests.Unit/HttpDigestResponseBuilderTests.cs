using System;
using System.Collections.Generic;
using System.Net.Http;
using NUnit.Framework;

namespace Bt.Web.Authentication.HttpDigest.Tests.Unit
{
    public class HttpDigestResponseBuilderTests
    {
        [TestCaseSource(typeof(HttpDigestAuthTestData), nameof(HttpDigestAuthTestData.Data))]
        public void BuildResponse_should_return_valid_response_Async(
            HashingAlgorithm hashingAlgorithm,
            string userName,
            string realm,
            string password,
            string nonce,
            HttpMethod httpMethod,
            Uri uri,
            Qop qop,
            ulong nonceCount,
            string cnonce,
            string expectedResponse)
        {
            // When
            var response = HttpDigestResponseBuilder.Build(
                hashingAlgorithm,
                userName,
                realm,
                password,
                nonce,
                httpMethod,
                uri,
                qop,
                nonceCount,
                cnonce);

            // Then
            Assert.That(response, Is.EqualTo(expectedResponse));
        }
    }

    internal class HttpDigestAuthTestData
    {
        internal static IEnumerable<TestCaseData> Data
        {
            get
            {
                // MD5
                yield return new TestCaseData(
                    HashingAlgorithm.Md5, // hashingAlgorithm
                    "Mufasa", // userName
                    "testrealm@host.com", // realm
                    "Circle Of Life", // password
                    "dcd98b7102dd2f0e8b11d0f600bfb0c093", // nonce
                    HttpMethod.Get, // httpMethod
                    new Uri("/dir/index.html", UriKind.Relative), // uri
                    Qop.Auth, // qop
                    1U, // nonceCount
                    "0a4f113b", // cnonce
                    "6629fae49393a05397450978507c4ef1" // expectedResponse
                );

                // SHA256
                yield return new TestCaseData(
                    HashingAlgorithm.Sha256, // hashingAlgorithm
                    "eperso", // userName
                    "Bt.Web.Service.Auth", // realm
                    "p@$$w0rd", // password
                    "3b90e6a96e3844288a88c4368c811e74", // nonce
                    HttpMethod.Get, // httpMethod
                    new Uri("/v1/token/new", UriKind.Relative), // uri
                    Qop.Auth, // qop
                    1U, // nonceCount
                    "b3d2e59b", // cnonce
                    "6bd93a0ff82b6cd0306ab3db5852ac0ac747cece579f97f5214a6652019b656e" // expectedResponse
                );
            }
        }
    }
}