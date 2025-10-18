using GarminFilter.Domain.Device.ValueObjects;

namespace GarminFilter.Domain.Device.Aggregates;

public record GarminDevice : IAggregate<DeviceId>
{
	public required string Name { get; init; }
	public required DeviceId Id { get; init; }

	public GarminDevice StripCharacters()
	{
		return this with
		{
			Name = Name
				.Replace("™", "")
				.Replace("®", "")
				.Replace("ē", "e")
		};
	}
}