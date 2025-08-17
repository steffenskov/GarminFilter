namespace GarminFilter.Infrastructure.Garmin.Services;

internal class GarminClient : IGarminClient
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public GarminClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
	{
		_httpClient = httpClient;
		_jsonSerializerOptions = jsonSerializerOptions;
	}

	public async Task<IEnumerable<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync("/deviceTypes", cancellationToken);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync(cancellationToken);

		return JsonSerializer.Deserialize<GarminDevice[]>(json, _jsonSerializerOptions) ?? throw new JsonException("Failed to deserialize devices");
	}

	public async Task<IEnumerable<GarminApp>> GetAppsAsync(int pageIndex, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync($"/apps?startPageIndex={pageIndex}&pageSize=30&sortType=mostRecent", cancellationToken);
		response.EnsureSuccessStatusCode();

		var json = await response.Content.ReadAsStringAsync(cancellationToken);

		return JsonSerializer.Deserialize<GarminApp[]>(json, _jsonSerializerOptions) ?? throw new JsonException("Failed to deserialize apps");
	}
}