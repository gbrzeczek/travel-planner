﻿using System.Linq.Expressions;
using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Common.Contracts;
using TravelPlanner.Api.Common.Interfaces;
using TravelPlanner.Api.Common.Models;
using TravelPlanner.Api.Common.Extensions;
using TravelPlanner.Api.Infrastructure.Persistence;

namespace TravelPlanner.Api.Features.Trip;

public static class GetTrips
{
    public class Query : PaginationQuery, IRequest<PagedResponse<TripResponse>>
    {
    }
    
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            Include(new PaginationQueryValidator());
        }
    }
    
    public class Handler(TripContext context, ICurrentUser currentUser) : IRequestHandler<Query, PagedResponse<TripResponse>>
    {
        public async Task<PagedResponse<TripResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = context.Trips.AsNoTracking().Where(t => t.CreatedBy == currentUser.Id);
            
            var totalItems = await query.CountAsync(cancellationToken);
            
            Expression<Func<Entities.Trip, object>> keySelector = request.SortBy switch
            {
                "name" => x => x.Name,
                "startDate" => x => x.StartDate,
                "endDate" => x => x.EndDate,
                _ => x => x.Id
            };
            
            query = query.OrderBy(keySelector, request.SortDirection).Paginate(request);

            var items = await query.ProjectToType<TripResponse>().ToListAsync(cancellationToken);

            return request.MapToPagedResponse(items, totalItems);
        }
    }
}

public class GetTripsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/trips", async ([AsParameters] GetTripsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetTrips.Query>();
            var result = await sender.Send(query);
            return Results.Ok(result);
        }).RequireAuthorization();
    }
}

public class GetTripsRequest : PaginationRequest
{
    
}

public class TripResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
}