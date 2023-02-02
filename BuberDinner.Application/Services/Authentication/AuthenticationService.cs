using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.userRepository = userRepository;
        }

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            if (userRepository.GetUserByEmail(email) is not null)
            {
                throw new Exception("User with given eimail already exists.");
            }
            var user = new User()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };
            userRepository.Add(user);
            var token = jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(user, token);
        }

        public AuthenticationResult Login(string email, string password)
        {
            //Validation the user
            if (userRepository.GetUserByEmail(email) is not User user)
            {
                throw new Exception("User with given email does not exist.");
            }
            //Validating the password
            if (user.Password!=password)
            {
                throw new Exception("Invalid password");
            }
            //Creating token
            var token = jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(user,token);
        }
    }
}
