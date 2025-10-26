using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppPermissionViewModel
{
	public AppPermissionViewModel(AppPermission permission)
	{
		Permission = permission.PrimitiveValue;
		Description = permission.Description;
	}

	public string Description { get; }

	public string Permission { get; }
}