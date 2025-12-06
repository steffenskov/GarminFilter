using GarminFilter.Domain.App.Aggregates;
using GarminFilter.Domain.App.Repositories;
using GarminFilter.SharedKernel.App.Entities;

namespace GarminFilter.Domain.App.Commands;

public record AppUpsertCommand(IGarminApp App) : IRequest<AppAggregate>;

file sealed class Handler : IRequestHandler<AppUpsertCommand, AppAggregate>
{
	private readonly IAppRepository _repository;

	public Handler(IAppRepository repository)
	{
		_repository = repository;
	}

	public Task<AppAggregate> Handle(AppUpsertCommand request, CancellationToken cancellationToken)
	{
		var app = AppAggregate.FromGarmin(request.App);

		_repository.Upsert(app);
		return Task.FromResult(app);
	}
}