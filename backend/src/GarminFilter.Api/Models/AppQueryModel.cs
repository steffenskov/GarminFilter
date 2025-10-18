using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppQueryModel
{
	public required HashSet<AppPermission> ExcludePermissions { get; set; }
	public bool IncludePaid { get; set; }
	public int PageIndex { get; set; }
	public int PageSize { get; set; }
}