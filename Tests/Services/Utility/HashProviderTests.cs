namespace DotNetGroup.Tests.Services.Utility
{
    using System;
    using System.Security.Cryptography;

    using DotNetGroup.Services.Utililty;

    using NUnit.Framework;

    [TestFixture]
    public class HashProviderTests
    {
        [Test]
        public void Given_Null_Value_Compute_Hash_Throws()
        {
            var hashProvider = new HashProvider();

            Assert.Throws<ArgumentNullException>(() => hashProvider.ComputeHash(null));
        }

        [Test]
        public void Given_String_Compute_Hash_Calculates_Correct_MD5_Hash()
        {
            var dotNetGroupValue = "DotNetGroup";
            var dotNetGroupHash = "4779ccd6ffccfac7cf91cfe585d02db0";

            var hashProvider = new HashProvider(new MD5CryptoServiceProvider());
            var hash = hashProvider.ComputeHash(dotNetGroupValue);

            Assert.That(hash, Is.EqualTo(dotNetGroupHash));
        }
    }
}
