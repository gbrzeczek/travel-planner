using Microsoft.AspNetCore.Identity;

namespace TravelPlanner.Api.Entities;

public class User : IdentityUser<int>
{
    public ICollection<Trip> Trips { get; set; } = new List<Trip>();
}