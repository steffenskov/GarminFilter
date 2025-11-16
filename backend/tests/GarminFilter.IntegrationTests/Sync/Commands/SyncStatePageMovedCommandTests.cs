using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Commands;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.IntegrationTests.Sync.Commands;

public class SyncStatePageMovedCommandTests : BaseTests
{
	private readonly IMediator _mediator;
	private readonly ISyncStateRepository _repository;

	public SyncStatePageMovedCommandTests(ContainerFixture fixture) : base(fixture)
	{
		_mediator = fixture.Provider.GetRequiredService<IMediator>();
		_repository = fixture.Provider.GetRequiredService<ISyncStateRepository>();
	}

	[Fact]
	public async Task SyncStatePageMovedCommand_DoesNotExist_IsCreated()
	{
		// Arrange
		var command = new SyncStatePageMovedCommand(AppTypes.Widget, 20);

		// Act
		var result = await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(command.Type);
		Assert.NotNull(result);
		Assert.Equal(result, fetched);
		Assert.Equal(20, result.PageIndex);
	}

	[Fact]
	public async Task SyncStatePageMovedCommand_AlreadyExists_IsUpdated()
	{
		// Arrange
		_repository.Upsert(new SyncState
		{
			Id = AppTypes.DeviceApp
		});
		var command = new SyncStatePageMovedCommand(AppTypes.DeviceApp, 20);

		// Act
		var result = await _mediator.Send(command);

		// Assert
		var fetched = _repository.GetSingle(command.Type);
		Assert.NotNull(result);
		Assert.Equal(result, fetched);
		Assert.Equal(20, result.PageIndex);
	}
}