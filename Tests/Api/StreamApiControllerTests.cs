using System;
using Api.Controllers;
using Moq;
using NUnit.Framework;
using Services;
using Services.Model;

namespace Tests.Api
{
    [TestFixture]
    public class StreamApiControllerTests
    {
        [Test]
        public void StreamApiControler_Can_Be_Created_With_Default_Contructor()
        {
            new StreamApiController();
        }

        [Test]
        public void By_Calling_Get_With_Id_StreamApi_Called_Once()
        {
            var id = "id";
            var fakeStreamApi = new Mock<IStreamApi>();
            var streamApiController = new StreamApiController(fakeStreamApi.Object);
            
            streamApiController.Get(id);

            fakeStreamApi.Verify(a => a.Get(id), Times.Once());
        }

        [Test]
        public void By_Calling_Stream_With_No_Arguments_StreamApi_Get_With_Two_Overloads_Called_Once()
        {
            var fakeStreamApi = new Mock<IStreamApi>();
            var streamApiController = new StreamApiController(fakeStreamApi.Object);

            streamApiController.Stream();

            fakeStreamApi.Verify(a => a.Get(It.IsAny<DateTime>(), It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void By_Calling_Stream_With_Item_Type_StreamApi_Get_With_Three_Overloads_Called_Once()
        {
            var fakeStreamApi = new Mock<IStreamApi>();
            var streamApiController = new StreamApiController(fakeStreamApi.Object);

            streamApiController.Stream(type: ItemType.Twitter);

            fakeStreamApi.Verify(a => a.Get(It.IsAny<DateTime>(), ItemType.Twitter, It.IsAny<int>()), Times.Once());
        }
    }
}
