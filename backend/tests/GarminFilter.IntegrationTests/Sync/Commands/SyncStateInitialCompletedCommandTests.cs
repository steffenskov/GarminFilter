using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.Domain.Sync.Repositories;

namespace GarminFilter.IntegrationTests.Sync.Commands;

public class SyncStateInitialCompletedCommandTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly ISyncStateRepository _repository;

	public SyncStateInitialCompletedCommandTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<ISyncStateRepository>();
	}

	[Fact]
	public async Task SyncStateInitialCompletedCommand_DoesNotExist_IsCreated()
	{
		// Arrange
		var command = new SyncStateInitialCompletedCommand(AppTypes.DataField);

		// Act
		var result = await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(command.Type);
		Assert.NotNull(result);
		Assert.Equal(result, fetched);
		Assert.True(result.InitialSyncCompleted);
	}

	[Fact]
	public async Task SyncStateInitialCompletedCommand_AlreadyExists_IsUpdated()
	{
		// Arrange
		_repository.Upsert(new SyncState
		{
			Id = AppTypes.WatchFace
		});
		var command = new SyncStateInitialCompletedCommand(AppTypes.WatchFace);

		// Act
		var result = await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(command.Type);
		Assert.NotNull(result);
		Assert.Equal(result, fetched);
		Assert.True(result.InitialSyncCompleted);
	}
}