namespace TravelPlanner.Api.Contracts.Trip;

public class TripResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
}