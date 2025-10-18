using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Sync.Commands;

namespace GarminFilter.Domain.Sync.Aggregates;

public record SyncState : IAggregate<AppType>
{
	public int? PageIndex { get; private init; }
	public bool InitialSyncCompleted { get; private init; }
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
			InitialSyncCompleted = true
		};
	}
}