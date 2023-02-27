using FluentAssertions;
using System;
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
		var factory1 = type.GetPropertyGetter(nameof(TestClass.Value));
		var factory2 = type.GetPropertyGetter(nameof(TestClass.Value));

		// Assert
		factory1
			.Should()
			.Be(factory2);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFound()
	{
		// Arrange
		var func = () => typeof(TestClass).GetPropertyGetter("NotRealProperty");

		// Assert
		func
			.Should()
			.ThrowExactly<ArgumentException>();
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
