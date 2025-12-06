using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Domain.Sync.Commands;

public record SyncStateRenewCommand(AppType Type) : IRequest<SyncState>;

file sealed class Handler : IRequestHandler<SyncStateRenewCommand, SyncState>
{
	private readonly ISyncStateRepository _repository;

	public Handler(ISyncStateRepository repository)
	{
		_repository = repository;
	}

	public Task<SyncState> Handle(SyncStateRenewCommand request, CancellationToken cancellationToken)
	{
		var state = _repository.GetSingle(request.Type) ?? throw new ArgumentOutOfRangeException(nameof(request.Type), $"SyncState not found: {request.Type}");

		var mutatedState = state.With(request);

		_repository.Upsert(mutatedState);

		return Task.FromResult(mutatedState);
	}
}