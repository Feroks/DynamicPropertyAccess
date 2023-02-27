using FluentAssertions;
using Xunit;

namespace DynamicPropertyAccess.Tests.TypeExtensionsTests;

public class TypeExtensionsGetPropertyGetterShould
{
	[Fact]
	public void ReturnCachedValue()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var getter1 = type.GetPropertyGetter(nameof(TestClass.Value));
		var getter2 = type.GetPropertyGetter(nameof(TestClass.Value));

		// Assert
		getter1
			.Should()
			.Be(getter2);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFound()
	{
		// Arrange
		var func = () => typeof(TestClass).GetPropertyGetter("NotRealProperty");

		// Assert
		func
			.Should()
			.ThrowExactly<PropertyNotFoundException>();
	}

	[Fact]
	public void ThrowExceptionIfPropertyDoesNotHaveGetter()
	{
		// Act
		var func = () => typeof(TestClass).GetPropertyGetter(nameof(TestClass.PropertyWithoutGet));

		// Assert
		func
			.Should()
			.ThrowExactly<PropertyDoesNotHaveGetterException>();
	}
}
