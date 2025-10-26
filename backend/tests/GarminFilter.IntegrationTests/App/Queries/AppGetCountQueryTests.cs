using GarminFilter.Client.Entities;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.IntegrationTests.App.Queries;

public class AppGetCountQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IAppRepository _repository;

	public AppGetCountQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IAppRepository>();
	}

	[Fact]
	public async Task AppGetCountQuery_NoneExists_ReturnsZero()
	{
		// Arrange
		var query = new AppGetCountQuery(AppTypes.DataField);

		// Act
		var result = await _mediator.Send(query);

		// Assert
		Assert.Equal(0, result);
	}

	[Fact]
	public async Task AppGetCountQuery_SomeExists_ReturnsCount()
	{
		// Arrange
		var includedApp1 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};

		var includedApp2 = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		_repository.Upsert(AppAggregate.FromGarmin(includedApp1));
		_repository.Upsert(AppAggregate.FromGarmin(includedApp2));

		var query = new AppGetCountQuery(AppTypes.WatchFace);

		// Act
		var result = await _mediator.Send(query);

		// Assert
		Assert.InRange(result, 2, int.MaxValue); // Count could be anything since other tests also upsert watchface type data
	}
}