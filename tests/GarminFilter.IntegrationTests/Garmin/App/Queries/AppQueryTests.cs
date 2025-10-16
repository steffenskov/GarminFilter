using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Garmin.Commands.App;
using GarminFilter.Domain.Garmin.Queries.App;
using GarminFilter.Domain.Garmin.ValueObjects;

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

		await _mediator.Send(new AppUpsertCommand(includedApp1));
		await _mediator.Send(new AppUpsertCommand(includedApp2));
		await _mediator.Send(new AppUpsertCommand(otherTypeApp));
		await _mediator.Send(new AppUpsertCommand(incompatibleDeviceApp));

		var query = new AppQuery(myDevice, AppTypes.WatchFace);

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Equal(2, result.Count);
		Assert.Contains(result, app => app.Id == includedApp1.Id);
		Assert.Contains(result, app => app.Id == includedApp2.Id);
		Assert.DoesNotContain(result, app => app.Id == otherTypeApp.Id);
		Assert.DoesNotContain(result, app => app.Id == incompatibleDeviceApp.Id);
	}
}