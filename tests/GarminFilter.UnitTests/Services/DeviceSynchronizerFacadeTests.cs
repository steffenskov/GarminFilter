using FreeMediator;
using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.Commands;
using GarminFilter.Domain.Device.ValueObjects;
using GarminFilter.Domain.Services;
using GarminFilter.Infrastructure.Garmin.Services;

namespace GarminFilter.UnitTests.Services;

public class DeviceSynchronizerFacadeTests
{
	[Fact]
	public async Task SynchronizeAsync_NoData_DoesNothing()
	{
		// Arrange
		DeviceUpsertCommand? upsertCommand = null;
		var client = Substitute.For<IGarminClient>();
		client.GetDevicesAsync(Arg.Any<CancellationToken>()).Returns([]);

		var mediator = Substitute.For<IMediator>();
		mediator.When(mock => mock.Send(Arg.Any<DeviceUpsertCommand>(), Arg.Any<CancellationToken>()))
			.Do(callInfo => upsertCommand = callInfo.Arg<DeviceUpsertCommand>());

		var synchronizer = new DeviceSynchronizerFacade(client, mediator);

		// Act
		await synchronizer.SynchronizeAsync();

		// Assert
		Assert.Null(upsertCommand);
	}

	[Fact]
	public async Task SynchronizeAsync_HasData_UpsertsDevices()
	{
		// Arrange
		var device1 = new GarminDevice
		{
			Id = new DeviceId(Random.Shared.Next()),
			Name = "Device 1"
		};

		var device2 = new GarminDevice
		{
			Id = new DeviceId(Random.Shared.Next()),
			Name = "Device 2"
		};

		List<DeviceUpsertCommand> upsertCommands = [];
		var client = Substitute.For<IGarminClient>();
		client.GetDevicesAsync(Arg.Any<CancellationToken>()).Returns([device1, device2]);

		var mediator = Substitute.For<IMediator>();
		mediator.When(mock => mock.Send(Arg.Any<DeviceUpsertCommand>(), Arg.Any<CancellationToken>()))
			.Do(callInfo => upsertCommands.Add(callInfo.Arg<DeviceUpsertCommand>()));

		var synchronizer = new DeviceSynchronizerFacade(client, mediator);

		// Act
		await synchronizer.SynchronizeAsync();

		// Assert
		var upsertCommand = Assert.Single(upsertCommands);
		Assert.Contains(upsertCommand.Devices, device => device == device1);
		Assert.Contains(upsertCommand.Devices, device => device == device2);
	}
}