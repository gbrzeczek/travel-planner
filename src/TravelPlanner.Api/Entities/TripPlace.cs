namespace TravelPlanner.Api.Entities;

public class TripPlace
{
    public required int Id { get; set; }
    public required long OpenStreetMapId { get; set; }
    public required string Note { get; set; }
    public required int TripId { get; set; }
    public Trip Trip { get; set; } = default!;
}