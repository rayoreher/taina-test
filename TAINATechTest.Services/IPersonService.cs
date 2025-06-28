using System.Collections.Generic;
using TAINATechTest.Data.Models;

namespace TAINATechTest.Services
{
    public interface IPersonService
    {
        List<Person> GetAllPeople();
        Person GetPersonById(long id);

        public int? AddPerson(Person person);
    }
}
