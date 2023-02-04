using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository userRepository;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.userRepository = userRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (userRepository.GetUserByEmail(request.Email) is not null)
            {
                return Errors.User.DuplicateEmail;
            }
            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password
            };
            userRepository.Add(user);
            var token = jwtTokenGenerator.GenerateToken(user);
            return new AuthenticationResult(user, token);
        }
    }
}
