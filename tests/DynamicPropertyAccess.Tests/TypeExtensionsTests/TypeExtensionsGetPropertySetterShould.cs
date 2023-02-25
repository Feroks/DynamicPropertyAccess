using FluentAssertions;
using System;
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
		var factory1 = type.GetPropertySetter(nameof(TestClass.Value));
		var factory2 = type.GetPropertySetter(nameof(TestClass.Value));

		// Assert
		factory1
			.Should()
			.Be(factory2);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFound()
	{
		// Arrange
		var func = () => typeof(TestClass).GetPropertySetter("NotRealProperty");

		// Assert
		func
			.Should()
			.ThrowExactly<ArgumentException>();
	}
}
