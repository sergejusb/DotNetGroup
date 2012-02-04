using NUnit.Framework;

namespace Tests.Web.Mobile
{
    using AutoMapper;

    using global::Web.Mobile.Services;

    [TestFixture]
    public class AutoMapperTests
    {
        [Test]
        public void Automapper_Configuration_Should_Be_Valid()
        {
            MappingService.Init();
            Mapper.AssertConfigurationIsValid();
        }
    }
}