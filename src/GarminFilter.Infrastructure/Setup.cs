using System.Text.Json.Serialization;
using GarminFilter.Infrastructure.Garmin.Repositories;
using GarminFilter.Infrastructure.Garmin.Services;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using StrongTypedId.LiteDB;

namespace GarminFilter.Infrastructure;

public static class Setup
{
	public static IServiceCollection AddDomain(this IServiceCollection services, string dbName)
	{
		var mapper = StrongTypedLiteDB.CreateBsonMapper(typeof(DeviceId).Assembly);
		var db = new LiteDatabase(dbName, mapper);
		services.AddSingleton(db);
		services.AddSingleton<IGarminDeviceRepository, GarminDeviceRepository>();
		services.AddHttpClient<GarminClient>(config =>
		{
			config.BaseAddress = new Uri("https://apps.garmin.com/api/appsLibraryExternalServices/api/asw");
			config.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (X11; Linux x86_64; rv:144.0) Gecko/20100101 Firefox/144.0");
			//config.DefaultRequestHeaders.UserAgent.ParseAdd("GarminFilterBot/1.0 (+https://github.com/steffenskov/GarminFilter)"); // TODO: Add this once the GitHub repo is up
		});
		services.AddTransient<IGarminClient, GarminClient>();
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