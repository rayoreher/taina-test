using System.Collections.Generic;
using TAINATechTest.Data.Models;

namespace TAINATechTest.Data.Repositories
{
    public interface IPersonRepository
    {
        public List<Person> GetAll();

        public Person GetById(long id);

        public int? AddPerson(Person person);
    }
}
