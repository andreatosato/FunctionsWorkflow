using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionWorkflow
{
    public class ConfirmRegistrationDto
    {
        public RegisterUserDto CurrentUser { get; set; }
        public bool Esito { get; set; }
    }
}
