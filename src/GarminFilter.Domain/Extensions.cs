using System.ComponentModel;

namespace GarminFilter.Domain;

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
}