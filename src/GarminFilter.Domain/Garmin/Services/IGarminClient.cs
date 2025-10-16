using GarminFilter.Domain.Garmin.Aggregates;

namespace GarminFilter.Domain.Garmin.Services;

public interface IGarminClient
{
	Task<IList<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default);
	Task<IList<GarminApp>> GetAppsAsync(int pageIndex, AppType type, CancellationToken cancellationToken = default);
}