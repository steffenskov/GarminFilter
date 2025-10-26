using GarminFilter.Client.Entities;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Client.Services;

public interface IGarminClient
{
	Task<IList<GarminDevice>> GetDevicesAsync(CancellationToken cancellationToken = default);
	Task<IList<GarminApp>> GetAppsAsync(int pageIndex, AppType type, CancellationToken cancellationToken = default);
	Task<Stream> GetFileAsync(string fileId, CancellationToken cancellationToken = default);
}