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

	public IEnumerable<GarminApp> Query(DeviceId deviceId, AppType type, bool includePaid, ISet<AppPermission> excludePermissions)
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

		return _collection.Find(expressionBuilder.ToString());
	}

	public IEnumerable<GarminApp> GetByType(AppType type)
	{
		return _collection.Find(aggregate => aggregate.TypeId == type.PrimitiveValue);
	}

	public int GetCount(AppType type)
	{
		return _collection.Count(aggregate => aggregate.TypeId == type.PrimitiveValue);
	}

	public override GarminApp? GetSingle(AppId id)
	{
		return _collection.FindById(id.PrimitiveValue);
	}
}