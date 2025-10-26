using GarminFilter.Client.Entities;
using GarminFilter.Domain.Device.Commands;
using GarminFilter.Domain.Device.Repositories;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.IntegrationTests.Device.Commands;

public class DeviceUpsertCommandTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IDeviceRepository _repository;

	public DeviceUpsertCommandTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IDeviceRepository>();
	}

	[Fact]
	public async Task DeviceUpsertCommand_DidNotExist_IsCreated()
	{
		// Arrange
		var device = new GarminDevice
		{
			Name = "New device",
			Id = new DeviceId(Random.Shared.Next())
		};
		var command = new DeviceUpsertCommand(device);

		// Act
		await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(device.Id);
		Assert.NotNull(fetched);
		Assert.Equal(device.Id, fetched.Id);
		Assert.Equal(device.Name, fetched.Name);
	}

	[Fact]
	public async Task DeviceUpsertCommand_AlreadyExists_IsUpdated()
	{
		// Arrange
		var device = new GarminDevice
		{
			Name = "New device",
			Id = new DeviceId(Random.Shared.Next())
		};
		await _mediator.Send(new DeviceUpsertCommand(device));


		var updatedDevice = device with
		{
			Name = "Updated name"
		};
		var command = new DeviceUpsertCommand(updatedDevice);

		// Act
		await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(device.Id);
		Assert.NotNull(fetched);
		Assert.NotEqual(device.Name, fetched.Name);
		Assert.Equal(updatedDevice.Id, fetched.Id);
		Assert.Equal(updatedDevice.Name, fetched.Name);
	}

	[Fact]
	public async Task DeviceUpsertCommand_MultipleDevices_AllAreUpserted()
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
		var command = new DeviceUpsertCommand(device1, device2);

		// Act
		await _mediator.Send(command);

		// Assert
		var all = _repository.GetAll().ToList();
		Assert.Contains(all, device => device.Id == device1.Id);
		Assert.Contains(all, device => device.Id == device2.Id);
	}
}