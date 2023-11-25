using System.Reflection;
using TravelPlanner.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TravelPlanner.Api;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TripContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("TripDb")));

        return services;
    }
}