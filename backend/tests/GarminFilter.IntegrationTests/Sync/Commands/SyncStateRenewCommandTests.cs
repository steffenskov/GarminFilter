using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.IntegrationTests.Sync.Commands;

public class SyncStateRenewCommandTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly ISyncStateRepository _repository;

	public SyncStateRenewCommandTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<ISyncStateRepository>();
	}

	[Fact]
	public async Task SyncStateRenewCommand_DoesNotExist_Throws()
	{
		// Arrange
		var command = new SyncStateRenewCommand(AppTypes.Music);

		// Act && Assert
		var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _mediator.Send(command));

		// Assert
		Assert.StartsWith($"SyncState not found: {AppTypes.Music}", ex.Message);
	}

	[Fact]
	public async Task SyncStateRenewCommand_Exists_IsUpdated()
	{
		// Arrange
		_repository.Upsert(new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStateInitialCompletedCommand(AppTypes.WatchFace,TimeProvider.System.GetUtcNowDate())));

		var command = new SyncStateRenewCommand(AppTypes.WatchFace);
		// Act
		var result = await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(command.Type);
		Assert.NotNull(result);
		Assert.Equal(result, fetched);
		Assert.False(result.InitialSyncCompleted);
	}
}