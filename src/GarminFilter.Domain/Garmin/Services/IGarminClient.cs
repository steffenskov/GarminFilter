using GarminFilter.Domain.Garmin.Aggregates;

namespace GarminFilter.Domain.Garmin.Services;

public interface IGarminClient
{
	Task<IEnumerable<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<GarminApp>> GetAppsAsync(int pageIndex, CancellationToken cancellationToken = default);
}