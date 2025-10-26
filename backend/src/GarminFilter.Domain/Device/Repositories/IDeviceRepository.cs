using GarminFilter.Domain.Device.Aggregates;
using GarminFilter.Domain.Shared.Repositories;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Domain.Device.Repositories;

public interface IDeviceRepository : IAggregateRepository<DeviceAggregate, DeviceId>
{
}