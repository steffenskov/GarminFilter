using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.SharedKernel.App.Entities;

public interface IGarminApp
{
	HashSet<DeviceId> CompatibleDeviceTypeIds { get; }
	long ReleaseDate { get; }
	AppDeveloper? Developer { get; }
	HashSet<AppPermission> Permissions { get; }

	Guid IconFileId { get; }
	AppType TypeId { get; }
	List<AppLocalization> AppLocalizations { get; }

	AppId Id { get; }

	AppPricing? Pricing { get; }
	decimal AverageRating { get; }
	uint ReviewCount { get; }
	GarminPaymentModel PaymentModel { get; }
}