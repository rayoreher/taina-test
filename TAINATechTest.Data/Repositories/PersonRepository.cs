using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Microsoft.EntityFrameworkCore;
using TAINATechTest.Data.Data;
using TAINATechTest.Data.Models;


namespace TAINATechTest.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext _personContext;

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public PersonRepository(PersonContext personContext)
        {
            _personContext = personContext;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            try
            {
                List<Person> people = await _personContext.People.AsNoTracking().ToListAsync();
                
                _log.Debug($"Found {people?.Count} people");

                return people;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception("An error occurred while retrieving people.", ex);
            }
        }

        public Person GetById(long id)
        {
            try
            {
                Person person = _personContext.People?.FirstOrDefault(x => x.Id == id);

                _log.Debug($"Found {person?.FirstName}  {person?.LastName} for id: {id}");

                return person;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return null;
        }

        public int? AddPerson(Person person)
        {
            try
            {
                person.FirstName = null;
                _personContext.Add(person);
                int newId = _personContext.SaveChanges();

                return newId;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return null;
        }
    }
}
