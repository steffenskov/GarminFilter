namespace GarminFilter.Api.Endpoints;

public interface IEndpoint
{
	string GroupName { get; }
	void MapEndpoint(RouteGroupBuilder builder);
}