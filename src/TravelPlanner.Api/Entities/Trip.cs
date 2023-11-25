namespace TravelPlanner.Api.Entities;

public class Trip
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int UserId { get; set; }
    public User User { get; set; } = default!;
    public ICollection<TripPlace> TripPlaces { get; set; } = new List<TripPlace>();
}