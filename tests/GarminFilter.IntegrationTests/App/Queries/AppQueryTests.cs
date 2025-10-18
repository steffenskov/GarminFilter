using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.IntegrationTests.App.Queries;

public class AppQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IGarminAppRepository _repository;

	public AppQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IGarminAppRepository>();
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
		var incompatiblePermission1 = new GarminApp
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

		_repository.Upsert(includedApp1, includedApp2, otherTypeApp, incompatibleDeviceApp, incompatiblePermission1, incompatiblePermission2, paidApp);

		var query = new AppQuery(myDevice, AppTypes.WatchFace, false, [new AppPermission(AppPermissions.Sensor), new AppPermission(AppPermissions.BluetoothLowEnergy)], 0, int.MaxValue);

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Equal(2, result.Count);
		Assert.Contains(result, app => app.Id == includedApp1.Id);
		Assert.Contains(result, app => app.Id == includedApp2.Id);
		Assert.DoesNotContain(result, app => app.Id == otherTypeApp.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatibleDeviceApp.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatiblePermission1.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatiblePermission2.Id);
		Assert.DoesNotContain(result, app => app.Id == paidApp.Id);
	}
}