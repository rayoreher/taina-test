using System.Collections.Generic;
using TAINATechTest.Data.Models;
using TAINATechTest.Data.Repositories;

namespace TAINATechTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }


        public List<Person> GetAllPeople()
        {
            return _personRepository.GetAll();
        }

        public Person GetPersonById(long id)
        {
            return _personRepository.GetById(id);
        }

        public int? AddPerson(Person person)
        {
            return _personRepository.AddPerson(person);
        }
    }
}
