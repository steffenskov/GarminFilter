using GarminFilter.Client.Entities;
using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Queries;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.IntegrationTests.Device.Queries;

public class DeviceGetAllQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IDeviceRepository _repository;

	public DeviceGetAllQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IDeviceRepository>();
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

		var devices = new[] { device1, device2 }
			.Select(DeviceAggregate.FromGarmin);

		_repository.Upsert(devices);

		var query = new DeviceGetAllQuery();

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Contains(result, device => device.Id == device1.Id);
		Assert.Contains(result, device => device.Id == device2.Id);
	}
}