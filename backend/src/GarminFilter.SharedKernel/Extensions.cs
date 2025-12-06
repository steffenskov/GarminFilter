using System.ComponentModel;

namespace GarminFilter.SharedKernel;

public static class Extensions
{
	public static string GetDescription(this Enum e)
	{
		var type = e.GetType();

		var memInfo = type.GetMember(e.ToString());

		if (memInfo.Length <= 0)
		{
			return e.ToString();
		}

		var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

		return attrs.Length > 0
			? ((DescriptionAttribute)attrs[0]).Description
			: e.ToString();
	}

	public static DateOnly GetUtcNowDate(this TimeProvider provider)
	{
		return DateOnly.FromDateTime(provider.GetUtcNow().Date);
	}

	public static DateOnly GetUtcNowDate(this TimeProvider provider, int daysOffset)
	{
		return DateOnly.FromDateTime(provider.GetUtcNow().AddDays(daysOffset).Date);
	}
}