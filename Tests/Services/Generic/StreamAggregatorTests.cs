using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Services.Generic;
using Services.Model;

namespace Tests.Services.Generic
{
    [TestFixture]
    public class StreamAggregatorTests
    {
        [Test]
        public void StreamAggregator_Can_Be_Successfully_Created_With_Default_Constructor()
        {
            new StreamAggregator();
        }

        [Test]
        public void Given_2_Aggregators_GetLatest_Called_Once_For_Each_Aggregator()
        {
            var fakeAggregator1 = new Mock<IItemAggregator>(MockBehavior.Loose);
            var fakeAggregator2 = new Mock<IItemAggregator>(MockBehavior.Loose);

            var streamAggregator = new StreamAggregator(fakeAggregator1.Object, fakeAggregator2.Object);

            var items = streamAggregator.GetLatest(DateTime.MinValue);

            fakeAggregator1.Verify(a => a.GetLatest(DateTime.MinValue), Times.Once());
            fakeAggregator2.Verify(a => a.GetLatest(DateTime.MinValue), Times.Once());
        }

        [Test]
        public void Given_2_Aggregators_GetLatest_Returns_2_Sorted_Items_Ascending_By_Date()
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var beforeYesterday = DateTime.UtcNow.AddDays(-2);

            var fakeAggregator1 = new Mock<IItemAggregator>();
            fakeAggregator1.Setup(a => a.GetLatest(It.IsAny<DateTime>())).Returns(BuildItems(yesterday));
            var fakeAggregator2 = new Mock<IItemAggregator>();
            fakeAggregator2.Setup(a => a.GetLatest(It.IsAny<DateTime>())).Returns(BuildItems(beforeYesterday));

            var streamAggregator = new StreamAggregator(fakeAggregator1.Object, fakeAggregator2.Object);

            var items = streamAggregator.GetLatest(DateTime.MinValue).ToList();

            Assert.AreEqual(items.Count, 2);
            Assert.AreEqual(items[0].Published, beforeYesterday);
            Assert.AreEqual(items[1].Published, yesterday);
        }

        [Test]
        public void Given_Null_Argument_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new StreamAggregator(null));
        }

        private IEnumerable<Item> BuildItems(DateTime date)
        {
            return new Fixture()
                .Build<Item>()
                .Without(i => i.Id)
                .Without(i => i.Tags)
                .With(i => i.Published, date)
                .CreateMany(count: 1);
        }
    }
}
