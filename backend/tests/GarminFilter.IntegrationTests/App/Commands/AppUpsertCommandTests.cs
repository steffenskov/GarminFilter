using GarminFilter.Client.Entities;
using GarminFilter.Domain.App.Commands;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.IntegrationTests.App.Commands;

public class AppUpsertCommandTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IAppRepository _repository;

	public AppUpsertCommandTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IAppRepository>();
	}

	[Fact]
	public async Task AppUpsertCommand_DidNotExist_IsCreated()
	{
		// Arrange
		var app = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		var command = new AppUpsertCommand(app);

		// Act
		await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(app.Id);
		Assert.NotNull(fetched);
		Assert.Equal(app.TypeId, fetched.Type);
		Assert.Equal(app.Id, fetched.Id);
	}

	[Fact]
	public async Task AppUpsertCommand_AlreadyExists_IsUpdated()
	{
		// Arrange
		var app = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		await _mediator.Send(new AppUpsertCommand(app));

		var updatedApp = app with
		{
			Permissions = [new AppPermission(AppPermissions.Sensor)]
		};
		var command = new AppUpsertCommand(updatedApp);

		// Act
		await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(app.Id);
		Assert.NotNull(fetched);
		Assert.NotEqual(app.Permissions, fetched.RequiredPermissions);
		Assert.Equal(updatedApp.Permissions, fetched.RequiredPermissions);
	}
}