using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Api.Common.Exceptions;
using TravelPlanner.Api.Entities;

namespace TravelPlanner.Api.Auth;

public static class Login
{
    public class Command : IRequest
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
    
    public class Handler(SignInManager<User> signInManager) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;
            
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: true);
            
            if (!result.Succeeded)
            {
                throw new UnauthorizedException();
            }
        }
    }
}

public class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("account/login", async (
            [FromBody] LoginRequest request, 
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<Login.Command>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}