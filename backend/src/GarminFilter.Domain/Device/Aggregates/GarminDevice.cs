using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Domain.Device.Aggregates;

public record GarminDevice : IAggregate<DeviceId>
{
	public required string Name { get; init; }
	public string[] AdditionalNames { get; init; } = [];
	public string? UrlName { get; init; }
	public required DeviceId Id { get; init; }


	public virtual bool Equals(GarminDevice? other)
	{
		if (other is null)
			return false;
		
		return Id.Equals(other.Id) &&  Name.Equals(other.Name) && UrlName?.Equals(other.UrlName) != false && AdditionalNames.SequenceEqual(other.AdditionalNames);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, AdditionalNames, UrlName, Id);
	}
}