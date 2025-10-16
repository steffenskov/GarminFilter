using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;

namespace GarminFilter.Domain.App.Commands;

public record AppUpsertCommand(GarminApp App) : IRequest;

file sealed class Handler : IRequestHandler<AppUpsertCommand>
{
	private readonly IGarminAppRepository _repository;

	public Handler(IGarminAppRepository repository)
	{
		_repository = repository;
	}

	public Task Handle(AppUpsertCommand request, CancellationToken cancellationToken)
	{
		_repository.Upsert(request.App);
		return Task.CompletedTask;
	}
}