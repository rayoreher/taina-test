using System.Collections.Generic;
using System.Threading.Tasks;
using TAINATechTest.Data.Models;

namespace TAINATechTest.Data.Repositories
{
    public interface IPersonRepository
    {
        public Task<List<Person>> GetAllAsync();

        public Person GetById(long id);

        public int? AddPerson(Person person);
    }
}
