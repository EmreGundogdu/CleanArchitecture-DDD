﻿using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.RestAPI.Controllers
{
    [Route("[controller]")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender mediator;

        public AuthenticationController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var command = new RegisterCommand(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email, registerRequest.Password);
            ErrorOr<AuthenticationResult> registerResult = await mediator.Send(command);
            return registerResult.Match(registerResult => Ok(MapAuthResult(registerResult)),
                errors => Problem(errors));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var query = new LoginQuery(loginRequest.Email, loginRequest.Password);
            var result = await mediator.Send(query);
            if (result.IsError && result.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, title: result.FirstError.Description);
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
