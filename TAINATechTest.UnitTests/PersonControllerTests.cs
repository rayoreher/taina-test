using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAINATechTest.Controllers;
using TAINATechTest.Services;
using TAINATechTest.Services.Exceptions;
using TAINATechTest.Services.ViewModels;

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
        public async Task TestDetailsReturnNotFoundWhenIdNullAsync()
        {
            var result = await _personController.Details(null);
            Assert.IsTrue(result is NotFoundResult);
        }

        [Test]
        public async Task Index_ReturnsViewWithPeople_WhenServiceReturnsData()
        {
            // Arrange
            var expectedPeople = new List<ListPersonViewModel>
            {
                new ListPersonViewModel { Id = 1, FirstName = "John", LastName = "Doe" },
                new ListPersonViewModel { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };
            _personServiceMock.Setup(s => s.GetAllPeople()).ReturnsAsync(expectedPeople);

            // Act
            var result = await _personController.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<IEnumerable<ListPersonViewModel>>(viewResult.Model);
            var model = viewResult.Model as IEnumerable<ListPersonViewModel>;
            Assert.AreEqual(2, model.ToArray().Length);
        }

        [Test]
        public async Task Index_NavigateToError_WhenServiceThrowsExceptionAsync()
        {
            // Arrage
            _personServiceMock.Setup(s => s.GetAllPeople()).ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _personController.Index();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Error", redirectResult.ActionName);
        }

        [Test]
        public async Task Details_ReturnNotFound_WhenPassingNullToController()
        {
            // Act
            var result = await _personController.Details(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_NavigateToError_WhenServiceThrowsExceptionAsync()
        {
            // Arrage
            _personServiceMock.Setup(s => s.GetPersonById(10)).ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _personController.Details(10);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Error", redirectResult.ActionName);
        }

        [Test]
        public async Task Details_ReturnNotFound_WhenPersonNotFound()
        {
            // Arrange
            _personServiceMock.Setup(s => s.GetPersonById(10)).ThrowsAsync(new PersonNotFoundException(10));

            // Act
            var result = await _personController.Details(10);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnViewResultWithPerson_WhenPassingRightIdToController()
        {
            // Arrange
            var person = new DetailsPersonViewModel
            {
                EmailAddress = "test@test.com"
            };

            _personServiceMock.Setup(s => s.GetPersonById(1)).ReturnsAsync(person);

            // Act
            var result = await _personController.Details(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            var model = viewResult.Model as DetailsPersonViewModel;
            Assert.AreEqual("test@test.com", model.EmailAddress);
        }
    }
}