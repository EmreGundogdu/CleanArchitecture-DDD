using BuberDinner.Application.Services.Authentication.Commands;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Application.Services.Authentication.Queries;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.RestAPI.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationCommandService authenticationCommandService;
        private readonly IAuthenticationQueryService authenticationQueryService;

        public AuthenticationController(IAuthenticationCommandService authenticationCommandService, IAuthenticationQueryService authenticationQueryService)
        {
            this.authenticationCommandService = authenticationCommandService;
            this.authenticationQueryService = authenticationQueryService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            ErrorOr<AuthenticationResult> registerResult = authenticationCommandService.Register(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email, registerRequest.Password);
            return registerResult.Match(registerResult => Ok(MapAuthResult(registerResult)),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = authenticationQueryService.Login(loginRequest.Email, loginRequest.Password);
            if (result.IsError && result.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, title: result.FirstError.Description);
            }
            {

            }
            return result.Match(result => Ok(MapAuthResult(result)),
                errors => Problem(errors));
        }

        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
        }
    }
}
