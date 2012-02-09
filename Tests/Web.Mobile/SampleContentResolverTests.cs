using System.Linq;
using AutoMapper;
using NUnit.Framework;
using Services.Model;
using Web.Mobile.Models.ViewModels;
using Web.Mobile.Services;
using Web.Mobile.Services.Resolvers;

namespace Tests.Web.Mobile
{  
    [TestFixture]
    public class SampleContentResolverTests
    {
        [SetUp]
        public void Init()
        {
            MappingService.Init();            
        }

        [Test]
        public void Given_No_Content_After_Resolving_Return_No_Content()
        {
            var source = new Item
            {
                Content = null
            };

            var view = Mapper.Map<Item, ItemCompactView>(source);

            Assert.AreEqual(null, view.SampleContent);
        }

        [Test]         
        public void Given_Content_With_Html_After_Resolving_No_Html_Should_Be_Left()
        {
            var source = new Item
            {
                Content = "<b>test</b>"
            };

            var view = Mapper.Map<Item, ItemCompactView>(source);

            Assert.AreEqual("test", view.SampleContent);
        }

        [Test]
        public void Given_Content_Longer_Than_Cutoff_After_Resolving_Should_Be_Cut_To_NeareastWord()
        {
            var source = new Item
            {
                Content = string.Join(" ", Enumerable.Range(0, 100).Select(x => string.Format("[test{0}]", x)))
            };

            var view = Mapper.Map<Item, ItemCompactView>(source);

            Assert.IsTrue(view.SampleContent.EndsWith("] ..."), "Does not end with whole word or ...");
            Assert.IsTrue(view.SampleContent.Length <= SampleContentResolver.CutoffLength + 4, "Content was left too long");            
        }

        [Test]
        public void Given_Content_With_Extreemely_Long_Word_After_Resolving_Should_Be_Cut_To_CutoffLength()
        {
            var source = new Item
            {
                Content = "test " + new string('A', 200)
            };

            var view = Mapper.Map<Item, ItemCompactView>(source);
            
            Assert.IsTrue(view.SampleContent.Length <= SampleContentResolver.CutoffLength + 4, "Content was left too long");
            Assert.IsTrue(view.SampleContent.EndsWith("A ..."));
        }
    }
}