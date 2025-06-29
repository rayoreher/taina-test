using System.Collections.Generic;
using System.Threading.Tasks;
using TAINATechTest.Data.Models;

namespace TAINATechTest.Services
{
    public interface IPersonService
    {
        Task<List<Person>> GetAllPeople();
        Person GetPersonById(long id);

        public int? AddPerson(Person person);
    }
}
