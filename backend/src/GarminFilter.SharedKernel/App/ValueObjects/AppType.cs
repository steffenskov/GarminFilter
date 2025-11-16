namespace GarminFilter.SharedKernel.App.ValueObjects;

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

			throw new ArgumentException($"Invalid input value: {value}", nameof(value));
		}

		return Enum.Parse<AppTypes>(value);
	}

	public static implicit operator AppType(AppTypes value)
	{
		return new AppType(value);
	}

	public static implicit operator AppTypes(AppType value)
	{
		return value.PrimitiveEnumValue;
	}
}

public enum AppTypes
{
	WatchFace = 1,
	DeviceApp = 2,
	Widget = 3,
	DataField = 4,
	Music = 5
}