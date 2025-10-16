using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.UnitTests.Garmin.Aggregates;

public class GarminDeviceTests
{
	[Fact]
	public void GarminDevice_ValidJson_CanDeserialize()
	{
		// Arrange
		var json = """
		           {"id":"20","partNumber":"006-B2431-00","name":"Forerunner® 235","additionalNames":[],"imageUrl":"https://static.garmincdn.com/en/products/010-03717-54/v/cf-sm.jpg","urlName":"forerunner235"}
		           """;

		// Act
		var app = JsonSerializer.Deserialize<GarminDevice>(json, Setup.CreateJsonSerializerOptions());

		// Assert
		Assert.NotNull(app);
		Assert.Equal(DeviceId.Create(20), app.Id);
		Assert.Equal("Forerunner® 235", app.Name);
	}
}