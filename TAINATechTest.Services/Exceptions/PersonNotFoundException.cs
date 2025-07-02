using System;
using System.Collections.Generic;
using System.Text;

namespace TAINATechTest.Services.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException(long id) : base($"Person with id: {id} not found")
        {
            
        }
    }
}
