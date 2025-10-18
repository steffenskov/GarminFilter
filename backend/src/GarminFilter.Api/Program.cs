using Ckode;
using GarminFilter.Api.Endpoints;
using GarminFilter.Api.Services;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Infrastructure;
using GarminFilter.Infrastructure.Garmin.Policies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
	options.AddSchemaTransformer((schema, context, cancellationToken) =>
	{
		if (context.JsonTypeInfo.Type == typeof(HashSet<AppPermission>))
		{
			schema.Format = "string[]";
		}

		return Task.CompletedTask;
	});
});

builder.Services.AddLogging(config =>
{
	config.ClearProviders();
	config.AddConsole();
});
var dbPath = builder.Configuration["DatabasePath"] ?? throw new InvalidOperationException("Missing configuration: DatabasePath");
builder.Services.AddDomain($"{dbPath.Trim('/')}/garmin.db", new DelayPolicy(TimeSpan.FromSeconds(5)));

builder.Services.AddHostedService<ApiScraperService>();

builder.Services.AddCors(config =>
{
	config.AddDefaultPolicy(policy =>
	{
		policy.AllowAnyHeader();
		policy.AllowAnyMethod();
		policy.AllowAnyOrigin();
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