using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Common.Exceptions;
using TravelPlanner.Api.Infrastructure.Persistence;

namespace TravelPlanner.Api.Features.TripPlace;

public static class RemoveTripPlace
{
    public class Command : IRequest
    {
        public int TripId { get; set; }
        public int TripPlaceId { get; set; }
    }
    
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.TripId).NotEmpty();
            RuleFor(x => x.TripPlaceId).NotEmpty();
        }
    }
    
    public class Handler(TripContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var tripPlace = await context.TripPlaces
                .Where(x => x.Id == request.TripPlaceId)
                .SingleOrDefaultAsync(cancellationToken);

            if (tripPlace is null)
            {
                throw new NotFoundException("trip place", request.TripPlaceId);
            }

            context.TripPlaces.Remove(tripPlace);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}

public class RemoveTripPlaceEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/trips/{tripId:int}/places/{tripPlaceId:int}", async (
                RemoveTripPlaceRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
        {
            var command = request.Adapt<RemoveTripPlace.Command>();
            await sender.Send(command, cancellationToken);
        });
    }
}

public class RemoveTripPlaceRequest
{
    [FromRoute]
    public int TripId { get; set; }
    [FromRoute]
    public int TripPlaceId { get; set; }
}