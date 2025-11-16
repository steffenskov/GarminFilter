using System.ComponentModel;

namespace GarminFilter.SharedKernel.App.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class AppPermission : StrongTypedEnumValue<AppPermission, AppPermissions>
{
	public AppPermission(AppPermissions value) : base(value)
	{
	}

	public AppPermission(string value) : base(value)
	{
	}

	public string Description => PrimitiveEnumValue.GetDescription();

	public static implicit operator AppPermission(AppPermissions value)
	{
		return new AppPermission(value);
	}
}

public enum AppPermissions
{
	[Description("Send/receive data via ANT radio to/from third-party sensors")]
	Ant,

	[Description("Run in the background when it is not active (potentially affecting battery life)")]
	Background,

	[Description("Bluetooth Low Energy communication")]
	BluetoothLowEnergy,

	[Description("Send/receive information to/from the Internet")]
	Communications,

	[Description("Broadcast data for watch faces to subscribe to")]
	ComplicationPublisher,

	[Description("Read and display data from other apps.")]
	ComplicationSubscriber,

	[Description("FIT files (activity recordings)")]
	Fit,

	[Description("Record high resolution sensor data to a FIT file (may dramatically increase activity file size)")]
	FitContributor,

	[Description("Display notifications when running in the foreground or background.")]
	Notifications,

	[Description("Saved information such as courses, routes, waypoints, tracks or workouts")]
	PersistedContent,

	[Description("Write locations to a device’s location list")]
	PersistedLocations,

	[Description("GPS location")] Positioning,

	[Description("Allow services to push information to this app")]
	PushNotification,

	[Description("Sensor data (i.e., ANT+, heart rate, compass)")]
	Sensor,

	[Description("Record additional information into activity files")]
	SensorLogging,

	[Description("Heart rate, barometer, temperature, and altitude history")]
	SensorHistory,

	[Description("Your Garmin Connect™ fitness profile")]
	UserProfile
}