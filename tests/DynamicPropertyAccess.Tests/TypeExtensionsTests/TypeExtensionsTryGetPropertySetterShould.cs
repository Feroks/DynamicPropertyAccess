using FluentAssertions;
using Xunit;

namespace DynamicPropertyAccess.Tests.TypeExtensionsTests;

public class TypeExtensionsTryGetPropertySetterShould
{
	[Fact]
	public void ReturnTrueIfPropertyFound()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var factory1 = type.TryGetPropertySetter(nameof(TestClass.Value), out _);

		// Assert
		factory1
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFalseIfPropertyNotFound()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var factory1 = type.TryGetPropertySetter("NotRealProperty", out _);

		// Assert
		factory1
			.Should()
			.BeFalse();
	}

	[Fact]
	public void ReturnCachedSetter()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		type.TryGetPropertySetter(nameof(TestClass.Value), out var getter1);
		type.TryGetPropertySetter(nameof(TestClass.Value), out var getter2);

		// Assert
		getter1
			.Should()
			.Be(getter2);
	}
}
