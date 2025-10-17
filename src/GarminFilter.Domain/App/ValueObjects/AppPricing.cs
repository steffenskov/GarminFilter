namespace GarminFilter.Domain.App.ValueObjects;

public record AppPricing
{
	public required string PartNumber { get; init; }
}