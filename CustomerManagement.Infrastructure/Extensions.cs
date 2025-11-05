using CustomerManagement.Core.Middleware;
using CustomerManagement.Infrastructure.Repositories;
using CustomerManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CustomerManagement.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddPersistentServices();
        services.AddApplicationRepositories();
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(e => e.MapControllers());
        return app;
    }

    public static IServiceCollection AddPersistentServices(this IServiceCollection services)
    {
        //ervices.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
        //services.AddScoped<IMultitenant, Multitenant>();
        //services.AddTransient<IDataAccess, DataAccess>();
        services.AddSingleton<ExceptionMiddleware>();
        return services;
    }

    public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
    {
        services.AddSingleton<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
