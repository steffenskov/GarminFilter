using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class DeviceAggregateRepository : BaseAggregateRepository<GarminDevice, DeviceId>, IDeviceAggregateRepository
{
	public DeviceAggregateRepository(LiteDatabase db) : base(db, "devices")
	{
	}
}