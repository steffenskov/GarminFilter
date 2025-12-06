using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.SharedKernel.App.ValueObjects;
using PerformantReflection;

namespace GarminFilter.UnitTests.Sync;

public class SyncStateTests
{
	[Fact]
	public void ShouldRenewFullSync_NotCompleted_ReturnsFalse()
	{
		// Arrange
		var syncState = new SyncState
		{
			Id = AppTypes.WatchFace
		};

		// Act
		var shouldRenew = syncState.ShouldRenewFullSync();

		// Assert
		Assert.False(shouldRenew);
	}

	/// <summary>
	/// This only happens when upgrading an existing DB, otherwise this state cannot occur - still better safe than sorry, so we're testing
	/// </summary>
	[Fact]
	public void ShouldRenewFullSync_CompletedWithoutDate_ReturnsTrue()
	{
		// Arrange
		var syncState = new SyncState
		{
			Id = AppTypes.WatchFace
		};

		var accessor = new ObjectAccessor(syncState);
		accessor.Properties[nameof(SyncState.InitialSyncCompleted)].SetValue(true);

		// Act
		var shouldRenew = syncState.ShouldRenewFullSync();

		// Assert
		Assert.True(shouldRenew);
	}
	
	[Fact]
	public void ShouldRenewFullSync_CompletedWithRecentDate_ReturnsFalse()
	{
		// Arrange
		var syncState = new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStateInitialCompletedCommand(AppTypes.WatchFace));

		// Act
		var shouldRenew = syncState.ShouldRenewFullSync();

		// Assert
		Assert.False(shouldRenew);
	}
	
	[Theory]
	[InlineData(5, false)]
	[InlineData(6, false)]
	[InlineData(7, true)]
	[InlineData(8, true)]
	public void ShouldRenewFullSync_CompletedNDaysAgo_ReturnsBasedOnAge(int days, bool expectedShouldRenew)
	{
		// Arrange
		var syncState = new SyncState
		{
			Id = AppTypes.WatchFace
		}.With(new SyncStateInitialCompletedCommand(AppTypes.WatchFace));

		var accessor = new ObjectAccessor(syncState);
		accessor.Properties[nameof(SyncState.LastFullSync)].SetValue(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-days)));
		
		// Act
		var shouldRenew = syncState.ShouldRenewFullSync();

		// Assert
		Assert.Equal(expectedShouldRenew, shouldRenew);
	}
}