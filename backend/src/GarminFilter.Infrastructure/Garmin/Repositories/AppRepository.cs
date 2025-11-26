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
		_collection.EnsureIndex(app => app.DeveloperName);
	}

	public bool Exists(AppId id)
	{
		return _collection.Exists(aggregate => aggregate.Id == id);
	}

	public int GetCount(AppType type)
	{
		return _collection.Count(aggregate => aggregate.Type == type.PrimitiveValue);
	}

	public IEnumerable<string> GetDevelopers()
	{
		var query = _collection
			.Query()
			.GroupBy(nameof(AppAggregate.DeveloperName))
			.Select("@key");

		return query
			.ToEnumerable()
			.Select(bson => bson["expr"].ToString().Trim('"'))
			.OrderBy(val => val);
	}

	public override AppAggregate? GetSingle(AppId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}

	public IEnumerable<AppAggregate> Query(DeviceId deviceId, AppType type, bool? paid, string? developer, ISet<AppPermission> excludePermissions, int pageIndex, int pageSize, AppOrder orderBy)
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

		if (paid is not null)
		{
			query = query.Where(app => app.IsPaid == paid.Value);
		}

		if (!string.IsNullOrWhiteSpace(developer))
		{
			query = query.Where(app => app.DeveloperName == developer);
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
			AppOrders.WeightedRating => nameof(AppAggregate.WeightedAverageRating),
			_ => throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null)
		};
	}
}