using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public record RegisterCommand(string FirstName,string LastName,string Email,string Password);
}
