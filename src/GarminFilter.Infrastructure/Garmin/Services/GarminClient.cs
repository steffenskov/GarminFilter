using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Services;

namespace GarminFilter.Infrastructure.Garmin.Services;

internal class GarminClient : IGarminClient
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public GarminClient(IHttpClientFactory httpClientFactory, JsonSerializerOptions jsonSerializerOptions)
	{
		_httpClientFactory = httpClientFactory;
		_jsonSerializerOptions = jsonSerializerOptions;
	}

	public async Task<IList<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default)
	{
		using var client = _httpClientFactory.CreateClient(nameof(GarminClient));

		var response = await client.GetAsync("deviceTypes", cancellationToken);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync(cancellationToken);

		return JsonSerializer.Deserialize<GarminDevice[]>(json, _jsonSerializerOptions) ?? throw new JsonException("Failed to deserialize devices");
	}

	public async Task<IList<GarminApp>> GetAppsAsync(int pageIndex, AppType type, CancellationToken cancellationToken = default)
	{
		var appType = FormatAppType(type);
		var url = $"apps?startPageIndex={pageIndex}&pageSize={Consts.App.PageSize}&sortType=mostRecent&appType={appType}";
		using var client = _httpClientFactory.CreateClient(nameof(GarminClient));
		var response = await client.GetAsync(url, cancellationToken);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync(cancellationToken);

		return JsonSerializer.Deserialize<GarminApp[]>(json, _jsonSerializerOptions) ?? throw new JsonException("Failed to deserialize apps");
	}

	private string FormatAppType(AppTypes type)
	{
		return type switch
		{
			AppTypes.DataField => "datafield",
			AppTypes.DeviceApp => "watch-app",
			AppTypes.Music => "audio-content-provider-app",
			AppTypes.WatchFace => "watchface",
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
	}
}