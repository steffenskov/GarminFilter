using FreeMediator;
using GarminFilter.Client;
using GarminFilter.Client.Entities;
using GarminFilter.Client.Services;
using GarminFilter.Domain.App.Commands;
using GarminFilter.Infrastructure.Garmin.Policies;
using GarminFilter.Infrastructure.Garmin.Services;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.UnitTests.Services;

public class BaseAppSynchronizerFacadeTests
{
	[Fact]
	public async Task SynchronizeAsync_NoData_DoesNothing()
	{
		// Arrange
		AppUpsertCommand? upsertCommand = null;
		var client = Substitute.For<IGarminClient>();
		client.GetAppsAsync(Arg.Any<int>(), Arg.Any<AppType>(), Arg.Any<CancellationToken>()).Returns([]);

		var mediator = Substitute.For<IMediator>();
		mediator.When(mock => mock.Send(Arg.Any<AppUpsertCommand>(), Arg.Any<CancellationToken>()))
			.Do(callInfo => upsertCommand = callInfo.Arg<AppUpsertCommand>());

		var synchronizer = new FakeAppSynchronizerFacade(client, mediator);

		// Act
		await synchronizer.SynchronizeAsync();

		// Assert
		Assert.Null(upsertCommand);
	}

	[Fact]
	public async Task SynchronizeAsync_HasData_Upserts()
	{
		// Arrange
		var app1 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		var app2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};

		List<AppUpsertCommand> upsertCommands = [];
		var client = Substitute.For<IGarminClient>();
		client.GetAppsAsync(0, Arg.Any<AppType>(), Arg.Any<CancellationToken>()).Returns([app1, app2]);

		var mediator = Substitute.For<IMediator>();
		mediator.When(mock => mock.Send(Arg.Any<AppUpsertCommand>(), Arg.Any<CancellationToken>()))
			.Do(callInfo => upsertCommands.Add(callInfo.Arg<AppUpsertCommand>()));

		var synchronizer = new FakeAppSynchronizerFacade(client, mediator);

		// Act
		await synchronizer.SynchronizeAsync();

		// Assert
		Assert.NotEmpty(upsertCommands);
		Assert.Contains(upsertCommands, c => (GarminApp)c.App == app1);
		Assert.Contains(upsertCommands, c => (GarminApp)c.App == app2);
	}

	[Fact]
	public async Task SynchronizeAsync_HasFirstPage_QueriesSecondPage()
	{
		// Arrange
		var app1 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		var app2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		var queriedSecondTime = false;
		List<AppUpsertCommand> upsertCommands = [];
		var client = Substitute.For<IGarminClient>();
		client.GetAppsAsync(0, Arg.Any<AppType>(), Arg.Any<CancellationToken>()).Returns([app1, app2]);
		client.When(mock => mock.GetAppsAsync(Consts.App.PageSize, Arg.Any<AppType>(), Arg.Any<CancellationToken>()))
			.Do(_ =>
			{
				queriedSecondTime = true;
			});

		var mediator = Substitute.For<IMediator>();

		var synchronizer = new FakeAppSynchronizerFacade(client, mediator);

		// Act
		await synchronizer.SynchronizeAsync();

		// Assert
		Assert.True(queriedSecondTime);
		await client.Received(2).GetAppsAsync(Arg.Any<int>(), Arg.Any<AppType>(), Arg.Any<CancellationToken>());
	}
}

file class FakeAppSynchronizerFacade : BaseAppSynchronizerFacade<FakeAppSynchronizerFacade>
{
	public FakeAppSynchronizerFacade(IGarminClient client, IMediator mediator) : base(client, mediator, AppTypes.WatchFace, new NoDelayPolicy())
	{
	}
}