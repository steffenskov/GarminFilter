using System.Net;
using System.Text.Json.Serialization;
using GarminFilter.Domain;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.Domain.Device.ValueObjects;
using GarminFilter.Domain.Policies;
using GarminFilter.Domain.Services;
using GarminFilter.Domain.Sync.Repositories;
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
		services.AddSingleton(delayPolicy);
		services.AddSingleton<IGarminDeviceRepository, GarminDeviceRepository>();
		services.AddSingleton<IGarminAppRepository, GarminAppRepository>();
		services.AddSingleton<ISyncStateRepository, SyncStateRepository>();

		services.AddHttpClient<GarminClient>(client =>
		{
			client.BaseAddress = new Uri("https://apps.garmin.com/api/appsLibraryExternalServices/api/asw/");
			client.DefaultRequestHeaders.Add("Accept", "application/json");
			client.DefaultRequestHeaders.UserAgent.ParseAdd("GarminFilterBot/1.0 (+https://github.com/steffenskov/GarminFilter)");
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