using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Shared.Repositories;

namespace GarminFilter.Domain.Garmin.Repositories;

public interface IDeviceAggregateRepository : IAggregateRepository<GarminDevice, DeviceId>
{
}