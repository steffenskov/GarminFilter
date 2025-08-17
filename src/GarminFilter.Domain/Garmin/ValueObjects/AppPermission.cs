namespace GarminFilter.Domain.Garmin.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class AppPermission : StrongTypedEnumValue<AppPermission, AppPermissions>
{
	public AppPermission(AppPermissions value) : base(value)
	{
	}

	public AppPermission(string value) : base(value)
	{
	}
}

public enum AppPermissions
{
	Positioning,
	Background,
	SensorHistory,
	UserProfile,
	Communications,
	ComplicationSubscriber
}