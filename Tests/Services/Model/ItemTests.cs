namespace DotNetGroup.Tests.Services.Model
{
    using System;
    using System.Security.Cryptography;

    using DotNetGroup.Services.Model;
    using DotNetGroup.Services.Utililty;

    using NUnit.Framework;

    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void Given_Url_Is_Not_Null_Id_Returns_MD5_Hash_Of_It()
        {
            var item = new Item { Url = "http://sergejus.blogas.lt" };
            var hash = new HashProvider(new MD5CryptoServiceProvider()).ComputeHash(item.Url);

            Assert.That(item.Id, Is.EqualTo(hash));
        }

        [Test]
        public void Given_Url_Is_Null_Id_Getter_Throws()
        {
            var item = new Item();

            Assert.Throws<NullReferenceException>(() => { var id = item.Id; });
        }
    }
}
