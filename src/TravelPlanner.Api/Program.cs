using Carter;
using Microsoft.AspNetCore.Identity;
using TravelPlanner.Api;
using TravelPlanner.Api.Entities;
using TravelPlanner.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCarter();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<TripContext>()
    .AddApiEndpoints();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
});

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.MapIdentityApi<User>();
app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
