using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository userRepository;

        public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            //Validation the user
            if (userRepository.GetUserByEmail(request.Email) is not User user)
            {
                return Errors.Authentication.InvalidCredentials;
            }
            //Validating the password
            if (user.Password != request.Password)
            {
                return new[] { Errors.Authentication.InvalidCredentials };
            }
            //Creating token
            var token = jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(user, token);
        }
    }
}
