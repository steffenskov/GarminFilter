using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.UnitTests.Garmin.ValueObjects;

public class AppTypeTests
{
	[Fact]
	public void AppType_FromString_GetsProperEnumValue()
	{
		// Arrange
		var apptype = new AppType(AppTypes.WatchFace.ToString());

		// Assert
		Assert.Equal(AppTypes.WatchFace, apptype.PrimitiveEnumValue);
		Assert.Equal(nameof(AppTypes.WatchFace), apptype.PrimitiveValue);
	}

	[Fact]
	public void AppType_FromInteger_GetsProperEnumValue()
	{
		// Arrange
		var apptype = new AppType(((int)AppTypes.WatchFace).ToString());

		// Assert
		Assert.Equal(AppTypes.WatchFace, apptype.PrimitiveEnumValue);
		Assert.Equal(nameof(AppTypes.WatchFace), apptype.PrimitiveValue);
	}
}