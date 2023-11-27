using System.Text;
using System.Text.Encodings.Web;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using TravelPlanner.Api.Auth.Utils;
using TravelPlanner.Api.Common.Exceptions;
using TravelPlanner.Api.Entities;

namespace TravelPlanner.Api.Auth;

public static class Register
{
    public class Command : IRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            
            RuleFor(x => x.Password).MinimumLength(8);
            RuleFor(x => x.Password).Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
            RuleFor(x => x.Password).Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.");
            RuleFor(x => x.Password).Matches("[0-9]").WithMessage("Password must contain at least one number.");
            RuleFor(x => x.Password).Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        }
    }
    
    public class Handler(
            UserManager<User> userManager, 
            IEmailSender<User> emailSender,
            IHttpContextAccessor contextAccessor)
        : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email
            };
            
            var existingUser = await userManager.FindByNameAsync(user.UserName);
            if (existingUser != null)
            {
                throw new ApplicationValidationException("Username is already taken.");
            }
            
            existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new ApplicationValidationException("Email is already taken.");
            }

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.ToString());
            }

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var userId = await userManager.GetUserIdAsync(user);

            var confirmEmailUrl = ConfirmationLinkBuilder.Build(contextAccessor.HttpContext!, userId, code);
            await emailSender.SendConfirmationLinkAsync(user, user.Email, HtmlEncoder.Default.Encode(confirmEmailUrl));
        }
    }
}

public class RegisterRequest
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
}

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("account/register", async (
                RegisterRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<Register.Command>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}