using CustomerManagement.Core.Mappers;

namespace CustomerManagement.API.Extensions;

public static class MapperExtensions
{
    public static IServiceCollection AddApplicationMappers(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<CustomerMapper>();
        });

        return services;
    }
}
