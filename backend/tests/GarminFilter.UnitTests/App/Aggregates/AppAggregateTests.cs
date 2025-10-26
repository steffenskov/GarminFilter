using GarminFilter.Client.Entities;
using GarminFilter.Domain.App.Aggregates;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.UnitTests.App.Aggregates;

public class AppAggregateTests
{
	[Fact]
	public void FromGarmin_HasPricing_SetsIsPaidTrue()
	{
		// Arrange
		var garminApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			Pricing = new AppPricing
			{
				PartNumber = ""
			}
		};

		// Act
		var app = AppAggregate.FromGarmin(garminApp);

		// Assert
		Assert.True(app.IsPaid);
	}

	[Fact]
	public void FromGarmin_HasPaymentModel_SetsIsPaidTrue()
	{
		// Arrange
		var garminApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			PaymentModel = GarminPaymentModel.ThirdPartyPayment
		};

		// Act
		var app = AppAggregate.FromGarmin(garminApp);

		// Assert
		Assert.True(app.IsPaid);
	}

	[Fact]
	public void FromGarmin_HasNoPricingAndNoPaymentModel_SetsIsPaidFalse()
	{
		// Arrange
		var garminApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};

		// Act
		var app = AppAggregate.FromGarmin(garminApp);

		// Assert
		Assert.False(app.IsPaid);
	}

	[Fact]
	public void FromGarmin_HasLocalizations_SetsName()
	{
		// Arrange
		var garminApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			AppLocalizations =
			[
				new AppLocalization("en", "First name", "Description"),
				new AppLocalization("da", "Andet navn", "Beskrivelse")
			]
		};

		// Act
		var app = AppAggregate.FromGarmin(garminApp);

		// Assert
		Assert.Equal("First name", app.Name);
	}

	[Fact]
	public void FromGarmin_HasNoLocalizations_SetsNameToUnknown()
	{
		// Arrange
		var garminApp = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};

		// Act
		var app = AppAggregate.FromGarmin(garminApp);

		// Assert
		Assert.Equal("Unknown Name", app.Name);
	}

	[Fact]
	public void FromGarmin_HasReviewsAndRating_CalculatesProperRatingSortKey()
	{
		// Arrange
		var garminApp = new GarminApp
		{
			AverageRating = 4.2m,
			ReviewCount = 1337,
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};

		// Act
		var app = AppAggregate.FromGarmin(garminApp);
		// Assert
		Assert.Equal(420_000_001_337UL, app.RatingSortKey);
	}
}