using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Garmin.Repositories;
using GarminFilter.Domain.Garmin.ValueObjects;

namespace GarminFilter.IntegrationTests.Garmin.Device.Repository;

public class DeviceRepositoryTests : BaseTests
{
	private readonly IGarminDeviceRepository _repository;

	public DeviceRepositoryTests(ContainerFixture fixture) : base(fixture)
	{
		_repository = Provider.GetRequiredService<IGarminDeviceRepository>();
	}

	[Fact]
	public void Upsert_ValidEntity_IsUpserted()
	{
		// Arrange
		var entity = new GarminDevice
		{
			Id = new DeviceId(Random.Shared.Next()),
			Name = "Forerunner"
		};

		// Act
		_repository.Upsert(entity);

		// Assert
		Assert.Equal(entity, _repository.GetSingle(entity.Id));
	}
}