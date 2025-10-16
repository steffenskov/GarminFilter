namespace GarminFilter.IntegrationTests.Configuration;

[Collection(Consts.Collection)]
public abstract class BaseTests
{
	private readonly ContainerFixture _fixture;

	protected BaseTests(ContainerFixture fixture)
	{
		_fixture = fixture;
	}

	public ServiceProvider Provider => _fixture.Provider;
}

[CollectionDefinition(Consts.Collection)]
public class ContainerDefinition : ICollectionFixture<ContainerFixture>
{
}