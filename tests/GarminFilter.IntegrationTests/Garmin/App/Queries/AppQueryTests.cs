using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Commands;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.IntegrationTests.Garmin.App.Queries;

public class AppQueryTests : BaseTests
{
	private readonly IMediator _mediator;

	public AppQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
	}

	[Fact]
	public async Task AppQuery_SomeExists_ReturnsThose()
	{
		// Arrange
		var myDevice = new DeviceId(Random.Shared.Next());
		var otherDevice = new DeviceId(Random.Shared.Next());
		var includedApp1 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [myDevice]
		};

		var includedApp2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [otherDevice, myDevice]
		};

		var otherTypeApp = new GarminApp
		{
			TypeId = AppTypes.DataField,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [myDevice]
		};

		var incompatibleDeviceApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [otherDevice]
		};
		var incompatiblePermission = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [myDevice],
			Permissions = [new AppPermission(AppPermissions.Sensor)]
		};
		var incompatiblePermission2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [myDevice],
			Permissions = [new AppPermission(AppPermissions.BluetoothLowEnergy)]
		};

		var paidApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			CompatibleDeviceTypeIds = [myDevice],
			Pricing = new AppPricing
			{
				PartNumber = "foo"
			}
		};

		await _mediator.Send(new AppUpsertCommand(includedApp1));
		await _mediator.Send(new AppUpsertCommand(includedApp2));
		await _mediator.Send(new AppUpsertCommand(otherTypeApp));
		await _mediator.Send(new AppUpsertCommand(incompatibleDeviceApp));
		await _mediator.Send(new AppUpsertCommand(incompatiblePermission));
		await _mediator.Send(new AppUpsertCommand(incompatiblePermission2));
		await _mediator.Send(new AppUpsertCommand(paidApp));

		var query = new AppQuery(myDevice, AppTypes.WatchFace, false, [new AppPermission(AppPermissions.Sensor), new AppPermission(AppPermissions.BluetoothLowEnergy)]);

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Equal(2, result.Count);
		Assert.Contains(result, app => app.Id == includedApp1.Id);
		Assert.Contains(result, app => app.Id == includedApp2.Id);
		Assert.DoesNotContain(result, app => app.Id == otherTypeApp.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatibleDeviceApp.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatiblePermission.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatiblePermission2.Id);
		Assert.DoesNotContain(result, app => app.Id == paidApp.Id);
	}
}