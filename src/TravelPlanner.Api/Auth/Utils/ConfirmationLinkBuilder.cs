using Microsoft.AspNetCore.WebUtilities;

namespace TravelPlanner.Api.Auth.Utils;

public static class ConfirmationLinkBuilder
{
    public static string Build(HttpContext context, string userId, string code)
    {
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