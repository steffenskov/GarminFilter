using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class SyncStatusViewModel
{
	public SyncStatusViewModel(AppType type, SyncState? state, int count)
	{
		Type = type;
		InitialSyncCompleted = state?.InitialSyncCompleted == true;
		PageIndex = state?.PageIndex ?? 0;
		Count = count;
		LastFullSync = state?.LastFullSync;
	}

	public DateOnly? LastFullSync { get;  }

	public int Count { get; }
	public bool InitialSyncCompleted { get; }
	public int PageIndex { get; }

	public AppType Type { get; }
}