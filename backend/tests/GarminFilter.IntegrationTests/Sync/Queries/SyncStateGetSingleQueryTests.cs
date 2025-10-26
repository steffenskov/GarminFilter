using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Queries;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.IntegrationTests.Sync.Queries;

public class SyncStateGetSingleQueryTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly ISyncStateRepository _repository;

	public SyncStateGetSingleQueryTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<ISyncStateRepository>();
	}

	[Fact]
	public async Task SyncStateGetSingleQuery_Exists_ReturnsThat()
	{
		// Arrange
		var state = new SyncState
		{
			Id = AppTypes.Music
		};
		_repository.Upsert(state);
		var query = new SyncStateGetSingleQuery(AppTypes.Music);

		// Act
		var result = await _mediator.Send(query);

		// Assert
		Assert.Equal(state, result);
	}
}