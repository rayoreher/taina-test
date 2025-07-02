using System.Threading.Tasks;
using TAINATechTest.Data.Models;

namespace TAINATechTest.Data.Repositories
{
    public interface IPersonRepository
    {
        public Task<Person[]> GetAllAsync();

        public Task<Person?> GetByIdAsync(long id);

        Task<Person> AddPersonAsync(Person person);

        Task<Person> UpdatePersonAsync(Person person);
        Task DeletePersonAsync(Person person);
    }
}
