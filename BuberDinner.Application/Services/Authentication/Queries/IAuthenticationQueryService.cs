using ErrorOr;
using FluentResults;

namespace BuberDinner.Application.Services.Authentication.Commands
{
    public interface IAuthenticationQueryService
    {
        ErrorOr<AuthenticationResult> Login(string email, string password);
    }
}
