using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TAINATechTest.Models;
using TAINATechTest.Services;
using TAINATechTest.Services.Exceptions;
using TAINATechTest.Services.ViewModels;

namespace TAINATechTest.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(
            IPersonService personService,
            ILogger<PersonController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var people = await _personService.GetAllPeople();

                return View(people);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error while fetching all people in {methodName}", nameof(Index));
                return RedirectToAction(nameof(Error));
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var person = await _personService.GetPersonById(id.Value);

                return View(person);
            }
            catch (PersonNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error getting person details with id: {id} in {methodName}", id, nameof(Details));
                return RedirectToAction(nameof(Error));
            }

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePersonViewModel person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(person);
                }

                await _personService.AddPersonAsync(person);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error while creating a person in {methodName}", nameof(Create));
                return RedirectToAction(nameof(Error));
            }
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var person = await _personService.GetPersonById(id.Value);
                return View(new UpdatePersonViewModel
                {
                    Id = id.Value,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    EmailAddress = person.EmailAddress,
                    Gender = person.Gender,
                    PhoneNumber = person.PhoneNumber
                });
            }
            catch (PersonNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error while getting person by id: {id} in {methodName}", id, nameof(Edit));
                return RedirectToAction(nameof(Error));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePersonViewModel person)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(person);
                }

                await _personService.UpdatePersonAsync(person);
                return RedirectToAction(nameof(Index));
            }
            catch (PersonNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error while updating person with id: {id} in {methodName}", person.Id, nameof(Edit));
                return RedirectToAction(nameof(Error));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound(int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            return View();
        }
    }
}
