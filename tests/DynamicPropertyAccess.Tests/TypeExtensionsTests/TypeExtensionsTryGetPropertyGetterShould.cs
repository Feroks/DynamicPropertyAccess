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
		var factory1 = type.TryGetPropertyGetter(nameof(TestClass.Value), out _);

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
		var factory1 = type.TryGetPropertyGetter("NotRealProperty", out _);

		// Assert
		factory1
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
