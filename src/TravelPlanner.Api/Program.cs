using Carter;
using Serilog;
using TravelPlanner.Api;
using TravelPlanner.Api.Auth;
using TravelPlanner.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCarter();

builder.Services.AddAuthServices();

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseSerilogRequestLogging();

app.MapIdentityApi<User>();
app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
