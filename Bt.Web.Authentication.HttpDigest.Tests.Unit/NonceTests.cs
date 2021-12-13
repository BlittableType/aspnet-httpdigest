using System.Collections.Generic;
using NUnit.Framework;

namespace Bt.Web.Authentication.HttpDigest.Tests.Unit
{
    [TestFixture]
    public class NonceTests
    {
        [TestCaseSource(typeof(NonceTestsData), nameof(NonceTestsData.Equal))]
        public void Nonce_should_be_equal(Nonce a, Nonce b)
        {
            Assert.That(a, Is.EqualTo(b));
        }
        
        [TestCaseSource(typeof(NonceTestsData), nameof(NonceTestsData.NotEqual))]
        public void Nonce_should_be_not_equal(Nonce a, Nonce b)
        {
            Assert.That(a, Is.Not.EqualTo(b));
        }

        internal class NonceTestsData
        {
            internal static Nonce Sample = new Nonce(9000); 
            
            internal static IEnumerable<TestCaseData> Equal
            {
                get
                {
                    yield return new TestCaseData(new Nonce(1), new Nonce(1));
                    yield return new TestCaseData(Sample, Sample);
                    yield return new TestCaseData(null, null);
                }
            }
            
            internal static IEnumerable<TestCaseData> NotEqual
            {
                get
                {
                    yield return new TestCaseData(new Nonce(1), new Nonce(2));
                    yield return new TestCaseData(null, new Nonce(1));
                    yield return new TestCaseData(new Nonce(1), null);
                }
            }
        }
    }
}