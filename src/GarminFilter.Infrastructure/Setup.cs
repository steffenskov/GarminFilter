using System.Text.Json.Serialization;
using GarminFilter.Infrastructure.Garmin.Repositories;
using GarminFilter.Infrastructure.Garmin.Services;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace GarminFilter.Infrastructure;

public static class Setup
{
	public static IServiceCollection AddDomain(this IServiceCollection services)
	{
		var db = new LiteDatabase("GarminFilter.db");
		services.AddSingleton(db);
		services.AddSingleton<IDeviceAggregateRepository, DeviceAggregateRepository>();
		services.AddTransient<IGarminClient, GarminClient>();
		services.AddHttpClient<GarminClient>(config =>
		{
			config.BaseAddress = new Uri("https://apps.garmin.com/api/appsLibraryExternalServices/api/asw");
			//config.DefaultRequestHeaders.UserAgent.ParseAdd("GarminFilterBot/1.0 (+https://github.com/steffenskov/GarminFilter)"); // TODO: Add this once the GitHub repo is up
		});
		services.AddSingleton(CreateJsonSerializerOptions());

		return services;
	}

	static internal JsonSerializerOptions CreateJsonSerializerOptions()
	{
		return new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			NumberHandling = JsonNumberHandling.AllowReadingFromString
		};
	}
}