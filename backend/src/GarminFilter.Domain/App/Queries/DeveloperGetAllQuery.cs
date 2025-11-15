using GarminFilter.Domain.App.Repositories;

namespace GarminFilter.Domain.App.Queries;

public record DeveloperGetAllQuery : IRequest<IEnumerable<string>>;

file sealed class Handler : IRequestHandler<DeveloperGetAllQuery, IEnumerable<string>>
{
	private readonly IAppRepository _repository;

	public Handler(IAppRepository repository)
	{
		_repository = repository;
	}

	public Task<IEnumerable<string>> Handle(DeveloperGetAllQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_repository.GetDevelopers());
	}
}