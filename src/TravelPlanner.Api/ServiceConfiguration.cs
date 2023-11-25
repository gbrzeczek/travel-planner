using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using TravelPlanner.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Infrastructure.Email;

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
            options.UseSqlite(configuration.GetConnectionString("TripDb")))
            .Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)))
            .AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}