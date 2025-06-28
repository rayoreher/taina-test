
using System.ComponentModel;

namespace TAINATechTest.Data.Models
{
    public class Person
    {
        public long Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

    }
}
