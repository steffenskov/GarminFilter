using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Domain.Sync.Commands;

public record SyncStatePageMovedCommand(AppType Type, int? PageIndex) : IRequest<SyncState>;

file sealed class Handler : IRequestHandler<SyncStatePageMovedCommand, SyncState>
{
	private readonly ISyncStateRepository _repository;

	public Handler(ISyncStateRepository repository)
	{
		_repository = repository;
	}

	public Task<SyncState> Handle(SyncStatePageMovedCommand request, CancellationToken cancellationToken)
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