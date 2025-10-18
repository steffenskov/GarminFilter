using GarminFilter.Domain.App.Repositories;
using GarminFilter.Domain.App.ValueObjects;

namespace GarminFilter.Domain.App.Queries;

public record AppExistsQuery(AppId Id) : IRequest<bool>;

file sealed class Handler : IRequestHandler<AppExistsQuery, bool>
{
	private readonly IGarminAppRepository _repository;

	public Handler(IGarminAppRepository repository)
	{
		_repository = repository;
	}

	public Task<bool> Handle(AppExistsQuery request, CancellationToken cancellationToken)
	{
		var result = _repository.Exists(request.Id);
		return Task.FromResult(result);
	}
}