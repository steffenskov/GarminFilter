using System.Net;
using GarminFilter.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GarminFilter.Client;

public static class Setup
{
	public static IServiceCollection AddGarminClient(this IServiceCollection services)
	{
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

		return services;
	}
}