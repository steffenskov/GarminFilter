namespace GarminFilter.SharedKernel.App.ValueObjects;

[StrongTypedValueJsonConverterFactory]
public class AppOrder : StrongTypedEnumValue<AppOrder, AppOrders>
{
	public AppOrder(AppOrders value) : base(value)
	{
	}

	public AppOrder(string value) : base(value)
	{
	}

	public static implicit operator AppOrder(AppOrders value)
	{
		return new AppOrder(value);
	}
}

public enum AppOrders
{
	Newest,
	Rating,
	ReviewCount
}