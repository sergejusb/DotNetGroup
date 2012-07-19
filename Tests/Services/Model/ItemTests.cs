namespace DotNetGroup.Tests.Services.Model
{
    using System;

    using DotNetGroup.Services.Model;

    using NUnit.Framework;

    [TestFixture]
    public class ItemTests
    {
        [Test]
        public void Given_Invalid_Id_Format_Item_Throws()
        {
            var item = new Item();

            Assert.Throws<ArgumentException>(() => item.Id = "invalid_id_format");
        }
    }
}
