using TravelPlanner.Api.Common;

namespace TravelPlanner.Api.Entities;

public class Trip : AuditableEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public User User { get; set; } = default!;
    public ICollection<TripPlace> TripPlaces { get; set; } = new List<TripPlace>();
}