using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.Aggregates;

namespace GarminFilter.Domain.Services;

public interface IGarminClient
{
	Task<IList<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default);
	Task<IList<GarminApp>> GetAppsAsync(int pageIndex, AppType type, CancellationToken cancellationToken = default);
	Task<Stream> GetFileAsync(string fileId, CancellationToken cancellationToken = default);
}