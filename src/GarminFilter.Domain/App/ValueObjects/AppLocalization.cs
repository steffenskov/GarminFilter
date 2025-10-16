namespace GarminFilter.Domain.App.ValueObjects;

public class AppLocalization
{
	public required string Locale { get; init; }
	public required string Name { get; init; }
	public required string Description { get; init; }
}