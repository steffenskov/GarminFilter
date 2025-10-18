using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Domain.App.Aggregates;

public interface IGarminApp : IAggregate<AppId>
{
	AppPricing? Pricing { get; }
	List<AppLocalization> AppLocalizations { get; }
	AppType TypeId { get; }
	Guid IconFileId { get; }
	HashSet<AppPermission> Permissions { get; }
	AppDeveloper? Developer { get; }
	decimal AverageRating { get; }
	uint ReviewCount { get; }

	bool IsPaid => Pricing is not null;
	string Name => AppLocalizations.FirstOrDefault()?.Name ?? "Unknown";
	string DeveloperName => Developer?.DeveloperDisplayName ?? "Unknown";
}

public record GarminApp : IGarminApp
{
	public HashSet<DeviceId> CompatibleDeviceTypeIds { get; init; } = [];
	public ulong ReleaseDate { get; init; }
	public AppDeveloper? Developer { get; init; }
	public HashSet<AppPermission> Permissions { get; init; } = [];

	public Guid IconFileId { get; init; }
	public required AppType TypeId { get; init; }
	public List<AppLocalization> AppLocalizations { get; init; } = []; // TODO: Can we use a different type than List? LiteDB does not support ImmutableList etc.

	public required AppId Id { get; init; }

	public AppPricing? Pricing { get; init; }
	public decimal AverageRating { get; init; }
	public uint ReviewCount { get; init; }
}