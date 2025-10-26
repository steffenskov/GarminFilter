using GarminFilter.Domain.Sync.Aggregates;
using GarminFilter.Domain.Sync.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Domain.Sync.Queries;

public record SyncStateGetSingleQuery(AppType Type) : IRequest<SyncState?>;

file sealed class Handler : IRequestHandler<SyncStateGetSingleQuery, SyncState?>
{
	private readonly ISyncStateRepository _repository;

	public Handler(ISyncStateRepository repository)
	{
		_repository = repository;
	}

	public Task<SyncState?> Handle(SyncStateGetSingleQuery request, CancellationToken cancellationToken)
	{
		var result = _repository.GetSingle(request.Type);

		return Task.FromResult(result);
	}
}