using System.Text;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using TravelPlanner.Api.Common.Exceptions;
using TravelPlanner.Api.Entities;

namespace TravelPlanner.Api.Auth;

public static class ConfirmEmail
{
    public class Command : IRequest
    {
        public string UserId { get; init; } = string.Empty;
        public string Code { get; init; } = string.Empty;
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
    
    public class Handler(UserManager<User> userManager) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                // Avoid leaking information about the existence of a user
                throw new UnauthorizedException();
            }

            string code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            }
            catch
            {
                throw new UnauthorizedException();
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                throw new UnauthorizedException();
            }
        }
    }
}

public class ConfirmEmailRequest
{
    public required string UserId { get; init; }
    public required string Code { get; init; }
}

public class ConfirmEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("account/confirm-email", async (
            [AsParameters] ConfirmEmailRequest request, 
            [FromServices] ISender sender,
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<ConfirmEmail.Command>();
            await sender.Send(command, cancellationToken);
            return TypedResults.Text("Thank you for confirming your email.");
        });
    }
}