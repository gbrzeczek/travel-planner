using Microsoft.AspNetCore.Identity;
using TravelPlanner.Api.Entities;
using TravelPlanner.Api.Infrastructure.Persistence;

namespace TravelPlanner.Api.Auth;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        services.AddAuthorizationBuilder();

        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<TripContext>()
            .AddApiEndpoints();

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });
        
        return services;
    }
}