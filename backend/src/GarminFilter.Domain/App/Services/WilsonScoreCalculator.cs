namespace GarminFilter.Domain.App.Services;

public static class WilsonScoreCalculator
{
	public static decimal Calculate(decimal averageRating, uint reviewCount)
	{
		if (reviewCount == 0)
		{
			return 0;
		}

		var p = ((double)averageRating - 1) / 4.0;

		// Z-score for confidence level (1.96 for 95%)
		const double z = 1.96;

		// Wilson score interval lower bound
		var denominator = 1 + z * z / reviewCount;
		var score = (p + z * z / (2 * reviewCount) - z * Math.Sqrt((p * (1 - p) + z * z / (4 * reviewCount)) / reviewCount)) / denominator;

		return (decimal)(score * 4 + 1);
	}
}