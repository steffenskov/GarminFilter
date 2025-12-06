using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.UnitTests.Sync.Commands;

public class SyncStateRenewCommandTests
{
	[Fact]
	public void SyncStateRenewCommand_FinishedInitialSync_UpdatesState()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStateInitialCompletedCommand(AppTypes.WatchFace, TimeProvider.System.GetUtcNowDate()));

		var command = new SyncStateRenewCommand(AppTypes.WatchFace);

		// Act
		var result = aggregate.With(command);

		// Assert
		Assert.False(result.InitialSyncCompleted);
		Assert.Null(result.PageIndex);
	}

	[Fact]
	public void SyncStateRenewCommand_NotFinished_Throws()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		};

		var command = new SyncStateRenewCommand(AppTypes.WatchFace);

		// Act && Assert
		var ex = Assert.Throws<InvalidOperationException>(() => aggregate.With(command));

		Assert.Equal("Cannot renew a state that hasn't yet completed initial sync.", ex.Message);
	}
}