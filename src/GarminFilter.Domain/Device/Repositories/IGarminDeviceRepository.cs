using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Device.ValueObjects;
using GarminFilter.Domain.Shared.Repositories;

namespace GarminFilter.Domain.Device.Repositories;

public interface IGarminDeviceRepository : IAggregateRepository<GarminDevice, DeviceId>
{
}