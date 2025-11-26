using GarminFilter.Domain.App.Services;
using GarminFilter.SharedKernel.App.Entities;
using GarminFilter.SharedKernel.App.ValueObjects;
using GarminFilter.SharedKernel.Device.ValueObjects;

namespace GarminFilter.Domain.App.Aggregates;

public record AppAggregate : IAggregate<AppId>
{
	public HashSet<DeviceId> SupportedDevices { get; private init; } = [];
	public long ReleaseDate { get; private init; }

	public ulong RatingSortKey { get; private init; }
	public string DeveloperName { get; private init; } = default!;
	public HashSet<AppPermission> RequiredPermissions { get; private init; } = [];

	public Guid IconFileId { get; private init; }
	public AppType Type { get; private init; } = default!;
	public string Name { get; private init; } = default!;

	public bool IsPaid { get; private init; }
	public decimal AverageRating { get; private init; }
	public uint ReviewCount { get; private init; }

	public decimal WeightedAverageRating { get; private init; }

	public AppId Id { get; private init; } = default!;

	public static AppAggregate FromGarmin(IGarminApp garminApp)
	{
		return new AppAggregate
		{
			SupportedDevices = garminApp.CompatibleDeviceTypeIds,
			ReleaseDate = garminApp.ReleaseDate,
			DeveloperName = garminApp.Developer?.DeveloperDisplayName ?? "Unknown Developer",
			RequiredPermissions = garminApp.Permissions,
			IconFileId = garminApp.IconFileId,
			Type = garminApp.TypeId,
			Name = garminApp.AppLocalizations.FirstOrDefault()?.Name ?? "Unknown Name",
			Id = garminApp.Id,
			IsPaid = garminApp.Pricing is not null || garminApp.PaymentModel == GarminPaymentModel.ThirdPartyPayment,
			AverageRating = garminApp.AverageRating,
			ReviewCount = garminApp.ReviewCount,
			RatingSortKey = (ulong)(garminApp.AverageRating * 10) * 10000000000UL + garminApp.ReviewCount, // Average rating is a decimal with a single digit (e.g. 4.2)
			WeightedAverageRating = WilsonScoreCalculator.Calculate(garminApp.AverageRating, garminApp.ReviewCount)
		};
	}
}