using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Common.Exceptions;
using TravelPlanner.Api.Infrastructure.Persistence;

namespace TravelPlanner.Api.Features.TripPlace;

public static class AddTripPlace
{
    public class Command : IRequest<int>
    {
        public required long OpenStreetMapId { get; set; }
        public required string Note { get; set; }
        public required int TripId { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.OpenStreetMapId).NotEmpty();
            RuleFor(x => x.Note).MaximumLength(100);
            RuleFor(x => x.TripId).NotEmpty();
        }
    }
    
    public class Handler(TripContext context) : IRequestHandler<Command, int>
    {
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var trip = await context.Trips.SingleOrDefaultAsync(t => t.Id == request.TripId, cancellationToken: cancellationToken);
            if (trip == null)
            {
                throw new NotFoundException("trip", request.TripId);
            }
            
            var tripPlace = new Entities.TripPlace
            {
                OpenStreetMapId = request.OpenStreetMapId,
                Note = request.Note,
                TripId = trip.Id,
                Trip = trip
            };

            context.TripPlaces.Add(tripPlace);
            await context.SaveChangesAsync(cancellationToken);

            return tripPlace.Id;
        }
    }
}

public class AddTripPlaceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/trips/{tripId}/places", async (
                AddTripPlaceRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<AddTripPlace.Command>();
            var result = await sender.Send(command, cancellationToken);
            return new CreatedResult($"/api/trips/{request.TripId}/places/{result}", result);
        }).RequireAuthorization();
    }
}

public class AddTripPlaceRequest
{
    [FromRoute]
    public int TripId { get; set; }
    public required long OpenStreetMapId { get; set; }
    public required string Note { get; set; }
    public int PlaceId { get; set; }
}