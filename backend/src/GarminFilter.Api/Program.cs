using Ckode;
using GarminFilter.Api.Endpoints;
using GarminFilter.Api.Services;
using GarminFilter.Infrastructure;
using GarminFilter.Infrastructure.Garmin.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddLogging(config =>
{
	config.ClearProviders();
	config.AddConsole();
});
var dbPath = builder.Configuration["DatabasePath"] ?? throw new InvalidOperationException("Missing configuration: DatabasePath");
var origin = builder.Configuration["Origin"] ?? throw new InvalidOperationException("Missing configuration: Origin");
builder.Services.AddDomain($"{dbPath.Trim('/')}/garmin.db", new DelayPolicy(TimeSpan.FromSeconds(5)));

builder.Services.AddHostedService<ApiScraperService>();

builder.Services.AddCors(config =>
{
	config.AddDefaultPolicy(policy =>
	{
		policy.WithMethods("GET", "POST", "OPTIONS");
		policy.AllowAnyHeader();
		policy.WithOrigins(origin);
	});
});


var app = builder.Build();

app.UseCors();
app.MapOpenApi();
app.UseSwaggerUi(options =>
{
	options.DocumentPath = "openapi/v1.json";
});

app.UseHttpsRedirection();

var endpoints = ServiceLocator.CreateInstances<IEndpoint>();
foreach (var endpoint in endpoints)
{
	var routeBuilder = app.MapGroup(endpoint.GroupName);
	endpoint.MapEndpoint(routeBuilder);
}

await app.RunAsync();