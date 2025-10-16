using GarminFilter.Domain.Garmin.Aggregates;
using GarminFilter.Domain.Garmin.Repositories;

namespace GarminFilter.Domain.Garmin.Commands.App;

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