using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.UnitTests.Sync.Commands;

public class SyncStatePageMovedCommandTests
{
	[Fact]
	public void SyncStatePageMovedCommand_InitialSyncCompleted_Throws()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStateInitialCompletedCommand(AppTypes.WatchFace, TimeProvider.System.GetUtcNowDate()));

		var command = new SyncStatePageMovedCommand(AppTypes.WatchFace, 30);

		// Act && Assert
		var ex = Assert.Throws<InvalidOperationException>(() => aggregate.With(command));

		Assert.Equal("Cannot move page on a sync that has already been initially completed.", ex.Message);
	}

	[Fact]
	public void SyncStatePageMovedCommand_NoExistingPageIndex_SetsPageIndex()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		};

		var command = new SyncStatePageMovedCommand(AppTypes.WatchFace, 30);

		// Act
		var result = aggregate.With(command);

		// Assert
		Assert.Equal(30, result.PageIndex);
	}

	[Fact]
	public void SyncStatePageMovedCommand_HasExistingPageIndex_UpdatesPageIndex()
	{
		// Arrange
		var aggregate = new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStatePageMovedCommand(AppTypes.WatchFace, 20));

		var command = new SyncStatePageMovedCommand(AppTypes.WatchFace, 30);

		// Act
		var result = aggregate.With(command);

		// Assert
		Assert.Equal(20, aggregate.PageIndex);
		Assert.Equal(30, result.PageIndex);
	}
}