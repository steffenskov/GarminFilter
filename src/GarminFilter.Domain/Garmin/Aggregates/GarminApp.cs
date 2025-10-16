namespace GarminFilter.Domain.Garmin.Aggregates;

public record GarminApp : IAggregate<AppId>
{
	public required AppType TypeId { get; init; }
	public HashSet<AppPermission> Permissions { get; init; } = [];
	public List<AppLocalization> AppLocalizations { get; init; } = []; // TODO: Can we use a different type than List? LiteDB does not support ImmutableList etc.
	public HashSet<DeviceId> CompatibleDeviceTypeIds { get; init; } = [];

	public required AppId Id { get; init; }
}