using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.Entities;

namespace GarminFilter.Domain.App.Commands;

public record AppUpsertCommand(IGarminApp App) : IRequest;

file sealed class Handler : IRequestHandler<AppUpsertCommand>
{
	private readonly IAppRepository _repository;

	public Handler(IAppRepository repository)
	{
		_repository = repository;
	}

	public Task Handle(AppUpsertCommand request, CancellationToken cancellationToken)
	{
		var app = AppAggregate.FromGarmin(request.App);

		_repository.Upsert(app);
		return Task.CompletedTask;
	}
}