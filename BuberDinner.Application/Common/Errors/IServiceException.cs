using System.Net;

namespace BuberDinner.Application.Common.Errors
{
    public interface IServiceException
    {
        HttpStatusCode HttpStatusCode { get; }
        string ErrorMessage { get; }
    }
}
