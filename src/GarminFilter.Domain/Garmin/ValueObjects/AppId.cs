namespace GarminFilter.Domain.Garmin.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class AppId : StrongTypedGuid<AppId>
{
	public AppId(Guid primitiveValue) : base(primitiveValue)
	{
	}
}