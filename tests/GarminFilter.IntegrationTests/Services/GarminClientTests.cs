using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Services;

namespace GarminFilter.IntegrationTests.Services;

public class GarminClientTests : BaseTests
{
	private readonly IGarminClient _client;

	public GarminClientTests(ContainerFixture fixture) : base(fixture)
	{
		_client = Provider.GetRequiredService<IGarminClient>();
	}

	[Fact]
	public async Task GetDevicesAsync_Valid_ReturnsDevices()
	{
		// Act
		var devices = await _client.GetDevicesAsync();

		// Assert
		Assert.NotEmpty(devices);
	}

	[Fact]
	public async Task GetAppsAsync_Valid_ReturnsApps()
	{
		// Act
		var apps = await _client.GetAppsAsync(0, AppTypes.WatchFace);

		// Assert
		Assert.NotEmpty(apps);
	}
}