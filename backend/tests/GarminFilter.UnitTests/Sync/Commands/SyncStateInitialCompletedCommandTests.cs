using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.UnitTests.Sync.Commands;

public class SyncStateInitialCompletedCommandTests
{
	[Fact]
	public void SyncStateInitialCompletedCommand_HadPageIndex_PageIndexIsNulled()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStatePageMovedCommand(AppTypes.WatchFace, 20));

		var command = new SyncStateInitialCompletedCommand(AppTypes.WatchFace, TimeProvider.System.GetUtcNowDate());

		// Act
		var result = aggregate.With(command);

		// Assert
		Assert.Equal(20, aggregate.PageIndex);
		Assert.Null(result.PageIndex);
		Assert.True(result.InitialSyncCompleted);
	}

	[Fact]
	public void SyncStateInitialCompletedCommand_AlreadyCompleted_DoesNothing()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		};

		var command = new SyncStateInitialCompletedCommand(AppTypes.WatchFace, TimeProvider.System.GetUtcNowDate());
		aggregate = aggregate.With(command);

		// Act
		var result = aggregate.With(command);

		// Assert
		Assert.Equal(aggregate, result);
	}
}