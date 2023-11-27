using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TravelPlanner.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TravelPlanner.Api.Common;
using TravelPlanner.Api.Common.Behaviors;
using TravelPlanner.Api.Common.Interfaces;
using TravelPlanner.Api.Entities;
using TravelPlanner.Api.Infrastructure.Email;
using TravelPlanner.Api.Infrastructure.Persistence.Interceptors;
using TravelPlanner.Api.Infrastructure.Services;

namespace TravelPlanner.Api;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient<ICurrentUser, CurrentUser>();

        services.AddExceptionHandler<ExceptionHandler>();
        
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddDbContext<TripContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlite(configuration.GetConnectionString("TripDb"));
        });

        services.Configure<EmailSenderOptions>(configuration.GetSection(nameof(EmailSenderOptions)))
            .AddTransient<IEmailSender<User>, EmailSender>();
        
        return services;
    }
}