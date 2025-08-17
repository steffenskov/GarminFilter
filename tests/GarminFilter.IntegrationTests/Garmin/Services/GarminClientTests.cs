using GarminFilter.Domain.Garmin.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GarminFilter.IntegrationTests.Garmin.Services;

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
}