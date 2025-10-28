using CustomerManagement.Services;
using CustomerManagement.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManagement.Domain;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }
}
