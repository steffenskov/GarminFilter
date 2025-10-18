namespace GarminFilter.Api;

public static class Extensions
{
	public static string Capitalize(this string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			return value;
		}

		return char.ToUpper(value[0]) + value[1..];
	}
}