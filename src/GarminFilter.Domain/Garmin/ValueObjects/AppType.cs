namespace GarminFilter.Domain.Garmin.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class AppType : StrongTypedEnumValue<AppType, AppTypes>
{
	public AppType(AppTypes value) : base(value)
	{
	}

	public AppType(string value) : base(value)
	{
	}
}

public enum AppTypes
{
	WatchFace = 1
}