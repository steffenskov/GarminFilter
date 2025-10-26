using GarminFilter.Api.Models;
using GarminFilter.Client.Entities;
using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.UnitTests.Device.Models;

public class DeviceViewModelTests
{
	[Fact]
	public void Create_DeviceHasAdditionalNames_ReturnsMultipleModels()
	{
		// Arrange
		var garminDevice = new GarminDevice
		{
			Name = "Fenix 8",
			Id = DeviceId.Create(308),
			AdditionalNames = ["Quatix 8"]
		};

		var device = DeviceAggregate.FromGarmin(garminDevice);

		// Act
		var models = DeviceViewModel.Create(device).ToList();

		// Assert
		Assert.Contains(models, model => model.Name == "Fenix 8");
		Assert.Contains(models, model => model.Name == "Quatix 8");
	}

	[Fact]
	public void IncludeDistinctName_DeviceHasNoUrlName_ReturnsJustName()
	{
		// Arrange
		var garminDevice = new GarminDevice
		{
			Name = "Fenix 8",
			Id = DeviceId.Create(308)
		};

		var device = DeviceAggregate.FromGarmin(garminDevice);
		var model = DeviceViewModel.Create(device).Single();

		// Act
		var withDistinctName = model.IncludeDistinctName();

		// Assert
		Assert.Equal("Fenix 8", withDistinctName.Name);
	}

	[Fact]
	public void IncludeDistinctName_DeviceHasUrlName_ReturnsCombinedName()
	{
		// Arrange
		var garminDevice = new GarminDevice
		{
			Name = "Fenix 8",
			UrlName = "fenix-8",
			Id = DeviceId.Create(308)
		};

		var device = DeviceAggregate.FromGarmin(garminDevice);
		var model = DeviceViewModel.Create(device).Single();

		// Act
		var withDistinctName = model.IncludeDistinctName();

		// Assert
		Assert.Equal("Fenix 8 (fenix-8)", withDistinctName.Name);
	}
}