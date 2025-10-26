using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;
using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class AppRepository : BaseAggregateRepository<AppAggregate, AppId>, IAppRepository
{
	public AppRepository(LiteDatabase db) : base(db, "apps")
	{
		_collection.EnsureIndex(app => app.Type);
	}

	public bool Exists(AppId id)
	{
		return _collection.Exists(aggregate => aggregate.Id == id);
	}

	public int GetCount(AppType type)
	{
		return _collection.Count(aggregate => aggregate.Type == type.PrimitiveValue);
	}

	public override AppAggregate? GetSingle(AppId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}

	public IEnumerable<AppAggregate> Query(DeviceId deviceId, AppType type, bool includePaid, ISet<AppPermission> excludePermissions, int pageIndex, int pageSize, AppOrder orderBy)
	{
		ArgumentOutOfRangeException.ThrowIfNegative(pageIndex);
		ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageSize);

		var query = _collection
			.Query()
			.Where(app => app.Type == type)
			.Where(app => app.SupportedDevices.Contains(deviceId));

		foreach (var permission in excludePermissions)
		{
			query = query.Where(app => !app.RequiredPermissions.Contains(permission));
		}

		if (!includePaid)
		{
			query = query.Where(app => app.IsPaid == false);
		}

		var orderProperty = GetOrderBy(orderBy);

		var skip = checked(pageIndex * pageSize);
		return query
			.OrderByDescending($"$.{orderProperty}")
			.Skip(skip)
			.Limit(pageSize)
			.ToEnumerable();
	}

	private static string GetOrderBy(AppOrder orderBy)
	{
		return orderBy.PrimitiveEnumValue switch
		{
			AppOrders.ReviewCount => nameof(AppAggregate.ReviewCount),
			AppOrders.Rating => nameof(AppAggregate.RatingSortKey),
			AppOrders.Newest => nameof(AppAggregate.ReleaseDate),
			_ => throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null)
		};
	}
}