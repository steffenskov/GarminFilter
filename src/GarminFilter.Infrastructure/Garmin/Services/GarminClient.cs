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

	public async Task<IEnumerable<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default)
	{
		using var client = _httpClientFactory.CreateClient(nameof(GarminClient));
		var response = await client.GetAsync("/deviceTypes", cancellationToken);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync(cancellationToken);

		return JsonSerializer.Deserialize<GarminDevice[]>(json, _jsonSerializerOptions) ?? throw new JsonException("Failed to deserialize devices");
	}

	public async Task<IEnumerable<GarminApp>> GetAppsAsync(int pageIndex, CancellationToken cancellationToken = default)
	{
		using var client = _httpClientFactory.CreateClient(nameof(GarminClient));
		var response = await client.GetAsync($"/apps?startPageIndex={pageIndex}&pageSize=30&sortType=mostRecent", cancellationToken);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync(cancellationToken);

		return JsonSerializer.Deserialize<GarminApp[]>(json, _jsonSerializerOptions) ?? throw new JsonException("Failed to deserialize apps");
	}
}