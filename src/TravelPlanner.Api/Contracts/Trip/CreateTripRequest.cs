namespace TravelPlanner.Api.Contracts.Trip;

public class CreateTripRequest
{
    public required string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
}