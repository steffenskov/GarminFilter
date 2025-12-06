using System.ComponentModel;

namespace GarminFilter.UnitTests;

public class ExtensionsTests
{
	[Fact]
	public void GetDescription_HasDescriptionAttribute_ReturnsAttributeValue()
	{
		// Arrange
		var fake = FakeTypes.HasDescription;

		// Act
		var description = fake.GetDescription();

		// Assert
		Assert.Equal("Has description", description);
	}

	[Fact]
	public void GetDescription_HasNoDescription_ReturnsEnumValueName()
	{
		// Arrange
		var fake = FakeTypes.NoDescription;

		// Act
		var description = fake.GetDescription();

		// Assert
		Assert.Equal(fake.ToString(), description);
	}

	[Fact]
	public void GetDescription_NotMemberOfEnum_ReturnsNumericValueToString()
	{
		// Arrange
		var fake = (FakeTypes)int.MaxValue;

		// Act
		var description = fake.GetDescription();

		// Assert
		Assert.Equal(int.MaxValue.ToString(), description);
	}
}

file enum FakeTypes
{
	NoDescription,
	[Description("Has description")] HasDescription
}