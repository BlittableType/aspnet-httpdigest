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
                    "user", // userName
                    "Bt.Web.Service.Auth", // realm
                    "p@$$w0rd", // password
                    "c7ad95ede948e907", // nonce
                    HttpMethod.Get, // httpMethod
                    new Uri("/v1/token/new", UriKind.Relative), // uri
                    Qop.Auth, // qop
                    1U, // nonceCount
                    "28e063b5", // cnonce
                    "0a6eb26d5a8607e495f0a71832003a3e9350a9a5f98c6e065b6c4b74bdcd2962" // expectedResponse
                );
            }
        }
    }
}