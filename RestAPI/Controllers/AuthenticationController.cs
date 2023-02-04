using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using OneOf.Types;

namespace BuberDinner.RestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest registerRequest)
        {
            OneOf<AuthenticationResult, DuplicateEmailError> registerResult = authenticationService.Register(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email, registerRequest.Password);
            return registerResult.Match(
                authResult => Ok(MapAuthResult(authResult)),
                _ => Problem(statusCode:StatusCodes.Status409Conflict,title:"Email already exits."));
        }

        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(authResult.User.Id, authResult.User.FirstName, authResult.User.LastName, authResult.User.Email, authResult.Token);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = authenticationService.Login(loginRequest.Email, loginRequest.Password);
            var response = new AuthenticationResponse(result.User.Id, result.User.FirstName, result.User.LastName, result.User.Email, result.Token);
            return Ok(response);
        }
    }
}
