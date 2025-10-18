using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.Domain.App.Queries;

public record AppGetCountQuery(AppType Type) : IRequest<int>;

file sealed class Handler : IRequestHandler<AppGetCountQuery, int>
{
	private readonly IGarminAppRepository _repository;

	public Handler(IGarminAppRepository repository)
	{
		_repository = repository;
	}

	public Task<int> Handle(AppGetCountQuery request, CancellationToken cancellationToken)
	{
		var count = _repository.GetCount(request.Type);

		return Task.FromResult(count);
	}
}