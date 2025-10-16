using System.Net;
using System.Text.Json.Serialization;
using GarminFilter.Domain;
using GarminFilter.Domain.Garmin.Policies;
using GarminFilter.Infrastructure.Garmin.Policies;
using GarminFilter.Infrastructure.Garmin.Repositories;
using GarminFilter.Infrastructure.Garmin.Services;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using StrongTypedId.LiteDB;

namespace GarminFilter.Infrastructure;

public static class Setup
{
	public static IServiceCollection AddDomain(this IServiceCollection services, string dbName, IDelayPolicy delayPolicy)
	{
		var mapper = StrongTypedLiteDB.CreateBsonMapper(typeof(DeviceId).Assembly);
		var db = new LiteDatabase(dbName, mapper);
		services.AddSingleton(db);
		services.AddSingleton<IGarminDeviceRepository, GarminDeviceRepository>();
		services.AddSingleton<IGarminAppRepository, GarminAppRepository>();

		services.AddHttpClient<GarminClient>(client =>
		{
			client.BaseAddress = new Uri("https://apps.garmin.com/api/appsLibraryExternalServices/api/asw/");
			client.DefaultRequestHeaders.Add("User-Agent", "GarminFilterBot");
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			//config.DefaultRequestHeaders.UserAgent.ParseAdd("GarminFilterBot/1.0 (+https://github.com/steffenskov/GarminFilter)"); // TODO: Add this once the GitHub repo is up
		}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
		{
			AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
			AllowAutoRedirect = true,
			UseCookies = true
		});
		services.AddTransient<IGarminClient, GarminClient>();
		services.AddTransient<ISynchronizerFacade, DeviceSynchronizerFacade>();
		services.AddTransient<ISynchronizerFacade, WatchFaceSynchronizerFacade>();
		services.AddSingleton(CreateJsonSerializerOptions());

		services.AddMediator(config =>
		{
			config.RegisterServicesFromAssemblyContaining<IMediatorHookup>();
		});

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