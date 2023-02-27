using FluentAssertions;
using Xunit;

namespace DynamicPropertyAccess.Tests.TypeExtensionsTests;

public class TypeExtensionsGetPropertySetterShould
{
	[Fact]
	public void ReturnCachedValue()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var setter1 = type.GetPropertySetter(nameof(TestClass.Value));
		var setter2 = type.GetPropertySetter(nameof(TestClass.Value));

		// Assert
		setter1
			.Should()
			.Be(setter2);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFound()
	{
		// Arrange
		var func = () => typeof(TestClass).GetPropertySetter("NotRealProperty");

		// Assert
		func
			.Should()
			.ThrowExactly<PropertyNotFoundException>();
	}

	[Fact]
	public void ThrowExceptionIfPropertyDoesNotHaveSetter()
	{
		// Act
		var func = () => typeof(TestClass).GetPropertySetter(nameof(TestClass.PropertyWithoutSet));

		// Assert
		func
			.Should()
			.ThrowExactly<PropertyDoesNotHaveSetterException>();
	}
}
