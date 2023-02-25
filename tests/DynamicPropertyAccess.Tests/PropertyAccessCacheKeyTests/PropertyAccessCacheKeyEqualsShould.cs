using FluentAssertions;
using Xunit;

namespace DynamicPropertyAccess.Tests.PropertyAccessCacheKeyTests;

public class PropertyAccessCacheKeyEqualsShould
{
	[Fact]
	public void ReturnTrueIfPropertyNamesMatchAndTypeNot()
	{
		const string propertyName = "property";

		// Arrange
		var key1 = new PropertyAccessCacheKey(propertyName, typeof(object));
		var key2 = new PropertyAccessCacheKey(propertyName, typeof(string));

		// Act
		var result = key1.Equals(key2);

		// Assert
		result
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFalseIfPropertyNamesDoNotMatches()
	{
		// Arrange
		var key1 = new PropertyAccessCacheKey("property1", typeof(object));
		var key2 = new PropertyAccessCacheKey("property2", typeof(string));

		// Act
		var result = key1.Equals((object)key2);

		// Assert
		result
			.Should()
			.BeFalse();
	}
}
