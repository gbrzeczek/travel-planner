using System.Text;
using System.Text.Encodings.Web;
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

public static class SendConfirmationEmail
{
    public class Command : IRequest
    {
        public required string Email { get; init; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
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
            var user = await userManager.FindByEmailAsync(request.Email);
            
            if (user == null)
            {
                throw new ApplicationValidationException("User with given email does not exist.");
            }
            
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var userId = await userManager.GetUserIdAsync(user);

            var confirmEmailUrl = BuildConfirmationLink(userId, code);
            await emailSender.SendConfirmationLinkAsync(user, user.Email!, HtmlEncoder.Default.Encode(confirmEmailUrl));
        }
        
        private string BuildConfirmationLink(string userId, string code)
        {
            var context = contextAccessor.HttpContext!;
            
            var uriParams = new Dictionary<string, string?>()
            {
                ["userId"] = userId,
                ["code"] = code,
            };
            var baseUri = context.Request.Scheme + "://" + context.Request.Host;
            const string confirmEmailSuffix = "/account/confirm-email";
        
            var uri = new Uri(baseUri + confirmEmailSuffix);
            return QueryHelpers.AddQueryString(uri.ToString(), uriParams);
        }
    }
}

public class ResendConfirmationEmailRequest
{
    public required string Email { get; init; }
}

public class ResendConfirmationEmailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("account/resend-confirmation-email", async (
            [FromBody]ResendConfirmationEmailRequest request, 
            [FromServices]ISender sender, 
            CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<SendConfirmationEmail.Command>();
            await sender.Send(command, cancellationToken);
            return Results.Ok();
        });
    }
}