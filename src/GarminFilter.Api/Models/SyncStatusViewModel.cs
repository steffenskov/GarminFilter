using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Sync.Aggregates;

namespace GarminFilter.Api.Models;

public class SyncStatusViewModel
{
	public SyncStatusViewModel(AppType type, SyncState? state, int count)
	{
		Type = type;
		InitialSyncCompleted = state?.InitialSyncCompleted == true;
		Count = count;
	}

	public int Count { get; }
	public bool InitialSyncCompleted { get; }
	public AppType Type { get; }
}