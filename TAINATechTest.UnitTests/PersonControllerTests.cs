using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TAINATechTest.Controllers;
using TAINATechTest.Services;

namespace TAINATechTest.UnitTests
{
    public class PersonControllerTests
    {
        private Mock<IPersonService> _personServiceMock;
        private PersonController _personController;

        [SetUp]
        public void Setup()
        {
            _personServiceMock = new Mock<IPersonService>();
            _personController = new PersonController(_personServiceMock.Object);
        }

        [Test]
        public void TestDetailsReturnNotFoundWhenIdNull()
        {
            var result = _personController.Details(null);
            Assert.IsTrue(result is NotFoundResult);
        }
    }
}