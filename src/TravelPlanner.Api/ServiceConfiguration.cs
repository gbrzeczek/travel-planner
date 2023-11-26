using System.Reflection;
using Microsoft.AspNetCore.Identity.UI.Services;
using TravelPlanner.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelPlanner.Api.Common.Interfaces;
using TravelPlanner.Api.Infrastructure.Email;
using TravelPlanner.Api.Infrastructure.Services;

namespace TravelPlanner.Api;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddTransient<ICurrentUser, CurrentUser>();
        
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TripContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(configuration.GetConnectionString("TripDb"));
        });

        services.Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)))
            .AddTransient<IEmailSender, EmailSender>();
        
        return services;
    }
}