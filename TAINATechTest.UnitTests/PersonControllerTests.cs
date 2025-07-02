using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using TAINATechTest.Controllers;
using TAINATechTest.Services;

namespace TAINATechTest.UnitTests
{
    public class PersonControllerTests
    {
        private Mock<IPersonService> _personServiceMock;
        private Mock<ILogger<PersonController>> _logger;
        private PersonController _personController;

        [SetUp]
        public void Setup()
        {
            _personServiceMock = new Mock<IPersonService>();
            _logger = new Mock<ILogger<PersonController>>();
            _personController = new PersonController(_personServiceMock.Object, _logger.Object);
        }

        [Test]
        public void TestDetailsReturnNotFoundWhenIdNull()
        {
            var result = _personController.Details(null);
            Assert.IsTrue(result is NotFoundResult);
        }
    }
}