using System;
using Moq;
using NUnit.Framework;
using Services.Model;
using Services.Processors;

namespace Tests.Services.Processors
{
    [TestFixture]
    public class ItemProcessorTests
    {
        [Test]
        public void ItemProcessor_Can_Be_Successfully_Created_With_Default_Constructor()
        {
            new ItemProcessor();
        }

        [Test]
        public void Given_2_Processors_Process_Called_Once_For_Each_Processor()
        {
            var fakeProcessor1 = new Mock<IItemProcessor>(MockBehavior.Loose);
            var fakeProcessor2 = new Mock<IItemProcessor>(MockBehavior.Loose);

            var itemProcessor = new ItemProcessor(fakeProcessor1.Object, fakeProcessor2.Object);

            itemProcessor.Process(new Item());

            fakeProcessor1.Verify(p => p.Process(It.IsAny<Item>()), Times.Once());
            fakeProcessor2.Verify(p => p.Process(It.IsAny<Item>()), Times.Once());
        }

        [Test]
        public void Given_Null_Argument_Constructor_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemProcessor(null));
        }
    }
}
