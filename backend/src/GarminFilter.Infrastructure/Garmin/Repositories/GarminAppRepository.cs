using System.Text;
using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;
using LiteDB;

namespace GarminFilter.Infrastructure.Garmin.Repositories;

internal class GarminAppRepository : BaseAggregateRepository<GarminApp, AppId>, IGarminAppRepository
{
	public GarminAppRepository(LiteDatabase db) : base(db, "apps")
	{
		_collection.EnsureIndex(app => app.TypeId);
	}

	public bool Exists(AppId id)
	{
		return _collection.Exists(aggregate => aggregate.Id == id);
	}

	public IEnumerable<GarminApp> Query(DeviceId deviceId, AppType type, bool includePaid, ISet<AppPermission> excludePermissions, int pageIndex, int pageSize, AppOrder orderBy)
	{
		// Build the tags exclusion part
		var expressionBuilder = new StringBuilder();

		expressionBuilder.AppendLine($"$.{nameof(GarminApp.TypeId)} = '{type}'");
		expressionBuilder.AppendLine($"AND $.{nameof(GarminApp.CompatibleDeviceTypeIds)}[*] ANY = {deviceId}");

		foreach (var permission in excludePermissions)
		{
			expressionBuilder.AppendLine($"AND ($.{nameof(GarminApp.Permissions)}[*] ALL != '{permission}')");
		}

		if (!includePaid)
		{
			expressionBuilder.AppendLine($"AND $.{nameof(GarminApp.Pricing)} = NULL");
		}

		var orderProperty = GetOrderBy(orderBy);

		var skip = pageIndex * pageSize;

		return _collection
			.Query()
			.Where(expressionBuilder.ToString())
			.OrderByDescending($"$.{orderProperty}")
			.Skip(skip)
			.Limit(pageSize)
			.ToEnumerable();
	}

	public int GetCount(AppType type)
	{
		return _collection.Count(aggregate => aggregate.TypeId == type.PrimitiveValue);
	}

	public override GarminApp? GetSingle(AppId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}

	private static string GetOrderBy(AppOrder orderBy)
	{
		return orderBy.PrimitiveEnumValue switch
		{
			AppOrders.ReviewCount => nameof(GarminApp.ReviewCount),
			AppOrders.Rating => nameof(GarminApp.AverageRating),
			AppOrders.Newest => nameof(GarminApp.ReleaseDate),
			_ => throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null)
		};
	}
}