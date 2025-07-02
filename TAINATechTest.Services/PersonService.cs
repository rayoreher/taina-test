using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAINATechTest.Data.Models;
using TAINATechTest.Data.Repositories;
using TAINATechTest.Services.Exceptions;
using TAINATechTest.Services.ViewModels;

namespace TAINATechTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonService> _logger;

        public PersonService(
            IPersonRepository personRepository,
            ILogger<PersonService> logger)
        {
            _personRepository = personRepository;
            _logger = logger;
        }


        public async Task<IEnumerable<ListPersonViewModel>> GetAllPeople()
        {
            _logger.LogInformation("Getting all people for display");
            var people = await _personRepository.GetAllAsync();
            _logger.LogInformation("Number or people: {number}", people.Length);
            return people.Select(x => new ListPersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="PersonNotFoundException"></exception>
        public async Task<DetailsPersonViewModel> GetPersonById(long id)
        {
            _logger.LogInformation("Getting person with id: {id}", id);
            var person = await _personRepository.GetByIdAsync(id);

            if (person == null)
            {
                _logger.LogInformation("Person with id: {id} not found", id);
                throw new PersonNotFoundException(id);
            }

            return new DetailsPersonViewModel
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender,
                EmailAddress = person.EmailAddress,
                PhoneNumber = person.PhoneNumber
            };
        }

        public async Task<long> AddPersonAsync(CreatePersonViewModel person)
        {
            _logger.LogInformation("Creating new person");
            var newPerson = new Person
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Gender = person.Gender,
                EmailAddress = person.EmailAddress,
                PhoneNumber = person.PhoneNumber
            };
            await _personRepository.AddPersonAsync(newPerson);
            _logger.LogInformation("New person got assigned id: {id}", newPerson.Id);
            return newPerson.Id;
        }

        /// <summary>
        /// method for update a person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        /// <exception cref="PersonNotFoundException"></exception>
        public async Task<long> UpdatePersonAsync(UpdatePersonViewModel person)
        {
            _logger.LogInformation("Updating person with id: {id}", person.Id);

            var oldPerson = await _personRepository.GetByIdAsync(person.Id);

            if (oldPerson == null)
            {
                throw new PersonNotFoundException(person.Id);
            }

            oldPerson.FirstName = person.FirstName;
            oldPerson.LastName = person.LastName;
            oldPerson.Gender = person.Gender;
            oldPerson.EmailAddress = person.EmailAddress;
            oldPerson.PhoneNumber = person.PhoneNumber;

            await _personRepository.UpdatePersonAsync(oldPerson);

            return oldPerson.Id;
        }

        public async Task<bool> DeletePersonAsync(long id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
            {
                throw new PersonNotFoundException(id);
            }

            await _personRepository.DeletePersonAsync(person);
            return true;
        }
    }
}
