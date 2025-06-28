using Microsoft.EntityFrameworkCore;
using TAINATechTest.Data.Models;

namespace TAINATechTest.Data.Data
{
    public class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
    }
}
