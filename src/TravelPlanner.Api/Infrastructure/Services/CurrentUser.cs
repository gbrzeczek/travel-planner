using System.Security.Claims;
using TravelPlanner.Api.Common.Interfaces;

namespace TravelPlanner.Api.Infrastructure.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public int Id => int.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}