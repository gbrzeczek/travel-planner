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

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCarter();

builder.Services.AddAuthServices();

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", corsPolicyBuilder =>
{
    corsPolicyBuilder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseExceptionHandler(opt => { });

app.UseSerilogRequestLogging();

app.MapCarter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
