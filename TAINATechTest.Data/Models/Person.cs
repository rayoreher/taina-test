
using System.ComponentModel;

namespace TAINATechTest.Data.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
