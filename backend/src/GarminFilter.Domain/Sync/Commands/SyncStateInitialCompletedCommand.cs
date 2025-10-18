using GarminFilter.Domain.App.ValueObjects;
using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Repositories;

namespace GarminFilter.Domain.Sync.Commands;

public record SyncStateInitialCompletedCommand(AppType Type) : IRequest<SyncState>;

file sealed class Handler : IRequestHandler<SyncStateInitialCompletedCommand, SyncState>
{
	private readonly ISyncStateRepository _repository;

	public Handler(ISyncStateRepository repository)
	{
		_repository = repository;
	}

	public Task<SyncState> Handle(SyncStateInitialCompletedCommand request, CancellationToken cancellationToken)
	{
		var state = _repository.GetSingle(request.Type) ?? new SyncState
		{
			Id = request.Type
		};

		var mutatedState = state.With(request);

		_repository.Upsert(mutatedState);

		return Task.FromResult(mutatedState);
	}
}