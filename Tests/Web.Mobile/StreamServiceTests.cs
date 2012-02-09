using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using Services.Model;
using Services.Web;
using Web.Mobile.Models;
using Web.Mobile.Services;

namespace Tests.Web.Mobile
{   
    [TestFixture]
    public class StreamServiceTests
    {
        [Test]
        public void Given_Filter_GetListUrl_Should_Produce_Correct_Url()
        {
            var filter = new StreamFilter
                {
                    From = new DateTime(2001, 1, 1),
                    Type = ItemType.Rss                    
                };

            var id = ObjectId.GenerateNewId();

            var jsonClient = new Mock<IJsonClient>();
            jsonClient.Setup(x => x.Get<IEnumerable<Item>>(It.IsAny<string>())).Returns(new List<Item> { new Item { Id = id.ToString() } });

            var streamService = new StreamService("http://api.dotnetgroup.dev", jsonClient.Object);
            var items = streamService.GetItems(filter).ToList();

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(id.ToString(), items[0].Id);
        }

        [Test]
        public void Given_Item_Id_GetItemUrl_Should_Produce_Correct_Url()
        {
            var id = ObjectId.GenerateNewId();

            var jsonClient = new Mock<IJsonClient>();
            jsonClient.Setup(x => x.Get<Item>(It.IsAny<string>())).Returns(new Item { Id = id.ToString() });

            var streamService = new StreamService("http://api.dotnetgroup.dev", jsonClient.Object);
            var item = streamService.GetItem("test");
            
            Assert.AreEqual(id.ToString(), item.Id);           
        }
    }
}