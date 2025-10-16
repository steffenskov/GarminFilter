namespace GarminFilter.Domain.App.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class AppType : StrongTypedEnumValue<AppType, AppTypes>
{
	public AppType(AppTypes value) : base(value)
	{
	}

	public AppType(string value) : base(SanitizeValue(value))
	{
	}

	private static AppTypes SanitizeValue(string value)
	{
		if (int.TryParse(value, out var numericValue))
		{
			if (Enum.IsDefined((AppTypes)numericValue))
			{
				return (AppTypes)numericValue;
			}

			throw new ArgumentException("Invalid input value: {value}", nameof(value));
		}

		return Enum.Parse<AppTypes>(value);
	}

	public static implicit operator AppType(AppTypes value)
	{
		return new AppType(value);
	}
}

public enum AppTypes
{
	WatchFace = 1,
	DataField = 2
}