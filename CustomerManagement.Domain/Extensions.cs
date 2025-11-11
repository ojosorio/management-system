using CustomerManagement.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using CustomerManagement.Domain.Services;

namespace CustomerManagement.Domain;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }

    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
        return services;
    }
}
