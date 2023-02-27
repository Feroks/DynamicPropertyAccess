using FluentAssertions;
using Xunit;

namespace DynamicPropertyAccess.Tests.TypeExtensionsTests;

public class TypeExtensionsTryGetPropertyGetterShould
{
	[Fact]
	public void ReturnTrueIfPropertyFound()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var getter = type.TryGetPropertyGetter(nameof(TestClass.Value), out _);

		// Assert
		getter
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFalseIfPropertyNotFound()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		var getter = type.TryGetPropertyGetter("NotRealProperty", out _);

		// Assert
		getter
			.Should()
			.BeFalse();
	}

	[Fact]
	public void ReturnCachedGetter()
	{
		// Arrange
		var type = typeof(TestClass);

		// Act
		type.TryGetPropertyGetter(nameof(TestClass.Value), out var getter1);
		type.TryGetPropertyGetter(nameof(TestClass.Value), out var getter2);

		// Assert
		getter1
			.Should()
			.Be(getter2);
	}
}
