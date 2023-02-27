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
		var setter = type.TryGetPropertySetter(nameof(TestClass.Value), out _);

		// Assert
		setter
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFalseIfPropertyNotFound()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var setter = type.TryGetPropertySetter("NotRealProperty", out _);

		// Assert
		setter
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
