using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.RestAPI.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender mediator;
        private readonly IMapper mapper;
        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var command = mapper.Map<RegisterCommand>(registerRequest);
            ErrorOr<AuthenticationResult> registerResult = await mediator.Send(command);
            return registerResult.Match(registerResult => Ok(mapper.Map<AuthenticationResponse>(registerResult)),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var query = mapper.Map<LoginQuery>(loginRequest);
            var result = await mediator.Send(query);
            if (result.IsError && result.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, title: result.FirstError.Description);
            }
            return result.Match(result => Ok(mapper.Map<AuthenticationResponse>(result)),
                errors => Problem(errors));
        }
    }
}
