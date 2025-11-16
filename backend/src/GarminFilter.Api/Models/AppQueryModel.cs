using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppQueryModel
{
	public required HashSet<AppPermission> ExcludePermissions { get; set; }
	public bool? Paid { get; set; }
	public string? Developer { get; set; }
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
	public AppOrder OrderBy { get; set; } = AppOrders.ReviewCount;
}