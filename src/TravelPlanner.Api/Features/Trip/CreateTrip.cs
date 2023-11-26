using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Api.Contracts.Trip;
using TravelPlanner.Api.Infrastructure.Persistence;

namespace TravelPlanner.Api.Features.Trip;

public static class CreateTrip
{
    public class Command : IRequest<int>
    {
        public required string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty();
            
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate);
        }
    }
    
    public class Handler(TripContext context) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var trip = new Entities.Trip
            {
                Name = request.Name,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            context.Trips.Add(trip);
            await context.SaveChangesAsync(cancellationToken);

            return trip.Id;
        }
    }
}

public class CreateTripEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/trips", async (
                CreateTripRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<CreateTrip.Command>();
            var result = await sender.Send(command, cancellationToken);
            return new CreatedResult($"/api/trips/{result}", result);
        }).RequireAuthorization();
    }
}