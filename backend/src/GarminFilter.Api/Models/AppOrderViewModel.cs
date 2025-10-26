using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppOrderViewModel
{
	public AppOrderViewModel(AppOrder order)
	{
		Name = order.PrimitiveValue;
	}

	public string Name { get; }
}