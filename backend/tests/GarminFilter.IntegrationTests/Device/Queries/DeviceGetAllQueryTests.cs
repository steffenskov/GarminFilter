using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Queries;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.IntegrationTests.Device.Queries;

public class DeviceGetAllQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IGarminDeviceRepository _repository;

	public DeviceGetAllQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IGarminDeviceRepository>();
	}

	[Fact]
	public async Task DeviceGetAllQuery_SomeExists_ReturnsThose()
	{
		// Arrange
		var device1 = new GarminDevice
		{
			Name = "New device",
			Id = new DeviceId(Random.Shared.Next())
		};
		var device2 = new GarminDevice
		{
			Name = "Second device",
			Id = new DeviceId(Random.Shared.Next())
		};
		_repository.Upsert(device1, device2);

		var query = new DeviceGetAllQuery();

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Contains(device1, result);
		Assert.Contains(device2, result);
	}
}