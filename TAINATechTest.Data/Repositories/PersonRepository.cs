using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TAINATechTest.Data.Data;
using TAINATechTest.Data.Models;


namespace TAINATechTest.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext _personContext;
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(
            PersonContext personContext,
            ILogger<PersonRepository> logger)
        {
            _personContext = personContext;
            _logger = logger;
        }

        public async Task<Person[]> GetAllAsync()
        {
            return await _personContext.People.AsNoTracking().ToArrayAsync();
        }

        public Task<Person?> GetByIdAsync(long id)
        {
            return _personContext.People.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Person> AddPersonAsync(Person person)
        {
                var addedEntity = _personContext.People.Add(person);
                await _personContext.SaveChangesAsync();

                return person;
        }

        public async Task<Person> UpdatePersonAsync(Person person)
        {
            _personContext.People.Update(person);
            await _personContext.SaveChangesAsync();
            return person;
        }

        public async Task DeletePersonAsync(Person person)
        {
            _personContext.People.Remove(person);
            await _personContext.SaveChangesAsync();

        }
    }
}
