namespace GarminFilter.Domain.Garmin.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class DeviceId : StrongTypedId<DeviceId, uint>
{
	public DeviceId(uint value) : base(value)
	{
	}
}