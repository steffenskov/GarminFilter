using System.Text.Json.Serialization;
using GarminFilter.Client;
using GarminFilter.Domain;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.Domain.Policies;
using GarminFilter.Domain.Services;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.Infrastructure.Garmin.Repositories;
using GarminFilter.Infrastructure.Garmin.Services;
using GarminFilter.SharedKernel.Device.ValueObjects;
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
		services.AddSingleton<IDeviceRepository, DeviceRepository>();
		services.AddSingleton<IAppRepository, AppRepository>();
		services.AddSingleton<ISyncStateRepository, SyncStateRepository>();


		services.AddTransient<ISynchronizerFacade, DeviceSynchronizerFacade>();
		services.AddTransient<ISynchronizerFacade, WatchFaceSynchronizerFacade>();
		services.AddSingleton(CreateJsonSerializerOptions());

		services.AddGarminClient();

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