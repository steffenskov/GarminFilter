namespace GarminFilter.SharedKernel.Device.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class DeviceId : StrongTypedId<DeviceId, int>
{
	public DeviceId(int value) : base(value)
	{
	}
}