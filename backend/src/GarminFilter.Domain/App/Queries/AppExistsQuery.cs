using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.ValueObjects;

namespace GarminFilter.Domain.App.Queries;

public record AppExistsQuery(AppId Id) : IRequest<bool>;

file sealed class Handler : IRequestHandler<AppExistsQuery, bool>
{
	private readonly IAppRepository _repository;

	public Handler(IAppRepository repository)
	{
		_repository = repository;
	}

	public Task<bool> Handle(AppExistsQuery request, CancellationToken cancellationToken)
	{
		var result = _repository.Exists(request.Id);
		return Task.FromResult(result);
	}
}