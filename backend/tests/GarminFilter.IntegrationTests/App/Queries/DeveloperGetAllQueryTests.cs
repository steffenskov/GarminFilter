using GarminFilter.Client.Entities;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.IntegrationTests.App.Queries;

public class DeveloperGetAllQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IAppRepository _repository;

	public DeveloperGetAllQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IAppRepository>();
	}

	[Fact]
	public async Task DeveloperGetAllQuery_DuplicatesExist_ReturnsDistinct()
	{
		// Arrange
		var app1 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			Developer = new AppDeveloper("Garmin")
		};

		var app2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			Developer = new AppDeveloper("Someone else")
		};

		var app3 = new GarminApp
		{
			TypeId = AppTypes.DataField,
			Id = AppId.New(),
			Developer = new AppDeveloper("Garmin")
		};

		var apps = new[] { app1, app2, app3 }
			.Select(AppAggregate.FromGarmin);

		_repository.Upsert(apps);

		var query = new DeveloperGetAllQuery();

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		Assert.Single(result, e => e == "Garmin");
		Assert.Single(result, e => e == "Someone else");
	}

	[Fact]
	public async Task DeveloperGetAllQuery_SomeExists_ReturnsOrdered()
	{
		// Arrange
		var app1 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			Developer = new AppDeveloper("Xyz")
		};

		var app2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New(),
			Developer = new AppDeveloper("Abc")
		};

		var apps = new[] { app1, app2 }
			.Select(AppAggregate.FromGarmin);

		_repository.Upsert(apps);

		var query = new DeveloperGetAllQuery();

		// Act
		var result = (await _mediator.Send(query)).ToList();

		// Assert
		Assert.NotEmpty(result);
		var indexOfXyz = result.IndexOf("Xyz");
		var indexOfAbc = result.IndexOf("Abc");
		Assert.True(indexOfXyz > indexOfAbc);
	}
}