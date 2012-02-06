namespace Tests.Services.Model
{
    using System;

    using NUnit.Framework;

    using global::Services.Model;

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
