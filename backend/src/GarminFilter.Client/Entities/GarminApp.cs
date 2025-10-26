using GarminFilter.SharedKernel.App.Entities;
using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Client.Entities;

public record GarminApp : IGarminApp
{
	public HashSet<DeviceId> CompatibleDeviceTypeIds { get; init; } = [];
	public long ReleaseDate { get; init; }
	public AppDeveloper? Developer { get; init; }
	public HashSet<AppPermission> Permissions { get; init; } = [];

	public Guid IconFileId { get; init; }
	public required AppType TypeId { get; init; }
	public List<AppLocalization> AppLocalizations { get; init; } = [];

	public required AppId Id { get; init; }

	public AppPricing? Pricing { get; init; }
	public decimal AverageRating { get; init; }
	public uint ReviewCount { get; init; }
	public GarminPaymentModel PaymentModel { get; init; }
}