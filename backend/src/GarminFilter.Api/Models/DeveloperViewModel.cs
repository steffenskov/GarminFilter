namespace GarminFilter.Api.Models;

public class DeveloperViewModel
{
	public DeveloperViewModel(string developer)
	{
		Name = developer;
	}

	public string Name { get; }
}