using GarminFilter.Domain.Sync.Commands;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Domain.Sync.Aggregates;

public record SyncState : IAggregate<AppType>
{
	public int? PageIndex { get; private init; }
	public bool InitialSyncCompleted { get; private init; }
	public DateOnly? LastFullSync { get; private init; }
	public required AppType Id { get; init; }

	public SyncState With(SyncStatePageMovedCommand command)
	{
		if (InitialSyncCompleted)
		{
			throw new InvalidOperationException("Cannot move page on a sync that has already been initially completed.");
		}

		return this with
		{
			PageIndex = command.PageIndex
		};
	}

	public SyncState With(SyncStateInitialCompletedCommand command)
	{
		return this with
		{
			PageIndex = null,
			InitialSyncCompleted = true,
			LastFullSync = command.CompletedAt
		};
	}

	public SyncState With(SyncStateRenewCommand command)
	{
		if (!InitialSyncCompleted)
		{
			throw new InvalidOperationException("Cannot renew a state that hasn't yet completed initial sync.");
		}

		return this with
		{
			PageIndex = null,
			InitialSyncCompleted = false
		};
	}
}

public static class SyncStateExtensions
{
	public static bool ShouldRenewFullSync(this SyncState state)
	{
		var today = DateOnly.FromDateTime(DateTime.UtcNow);

		return state.InitialSyncCompleted
		       && (state.LastFullSync is null || today.DayNumber - state.LastFullSync.Value.DayNumber >= 7);
	}
}