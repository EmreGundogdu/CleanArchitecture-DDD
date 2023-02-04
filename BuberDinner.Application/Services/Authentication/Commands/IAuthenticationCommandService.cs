using ErrorOr;
using FluentResults;

namespace BuberDinner.Application.Services.Authentication.Commands
{
    public interface IAuthenticationCommandService
    {
        ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
        ErrorOr<AuthenticationResult> Login(string email, string password);
    }
}
