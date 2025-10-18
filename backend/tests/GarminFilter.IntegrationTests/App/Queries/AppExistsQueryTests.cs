using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Queries;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.IntegrationTests.App.Queries;

public class AppExistsQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly IGarminAppRepository _repository;

	public AppExistsQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<IGarminAppRepository>();
	}

	[Fact]
	public async Task AppExistsQuery_DoesNotExist_ReturnsFalse()
	{
		// Arrange
		var query = new AppExistsQuery(AppId.New());

		// Act
		var result = await _mediator.Send(query);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public async Task AppExistsQuery_Exists_ReturnsTrue()
	{
		// Arrange
		var app = new GarminApp
		{
			TypeId = AppTypes.WatchFace,
			Id = AppId.New()
		};
		_repository.Upsert(app);

		var query = new AppExistsQuery(app.Id);

		// Act
		var result = await _mediator.Send(query);

		// Assert
		Assert.True(result);
	}
}