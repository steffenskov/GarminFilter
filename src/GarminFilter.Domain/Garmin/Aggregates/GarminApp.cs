namespace GarminFilter.Domain.Garmin.Aggregates;

public record GarminApp
{
	public required AppId Id { get; init; }
	public required AppType TypeId { get; init; }
	public ImmutableList<AppPermission> Permissions { get; init; } = [];
}