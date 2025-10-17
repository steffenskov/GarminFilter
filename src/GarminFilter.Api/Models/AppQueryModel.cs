using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.Api.Models;

public class AppQueryModel
{
	public HashSet<AppPermission> ExcludePermissions { get; set; } = [];
	public bool IncludePaid { get; set; }
}