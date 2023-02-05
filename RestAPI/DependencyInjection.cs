using BuberDinner.RestAPI.Common.Errors;
using BuberDinner.RestAPI.Common.Mapping;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BuberDinner.RestAPI;
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
        services.AddMappings();
        return services;
    }
}
