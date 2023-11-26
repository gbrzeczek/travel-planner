using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelPlanner.Api.Entities;

namespace TravelPlanner.Api.Infrastructure.Persistence;

public class TripContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<Trip> Trips  => Set<Trip>();
    public DbSet<TripPlace> TripPlaces => Set<TripPlace>();

    public TripContext(DbContextOptions<TripContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(TripContext).Assembly);
        
        base.OnModelCreating(builder);
    }
}
