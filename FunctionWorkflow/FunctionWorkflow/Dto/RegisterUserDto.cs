using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionWorkflow
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string IdConfirmationMail { get; set; }
        public string PhoneNumber { get; set; }
        
        // Keep calm, only for this example
        public string Password { get; set; }
    }
}
