using FluentAssertions;
using System;
using Xunit;

namespace DynamicPropertyAccess.Tests.ObjectExtensionTests;

public class ObjectExtensionsTrySetPropertyValueShould
{
	private const string? NewValue = "New Value";
	private readonly TestClass _model = new();

	[Fact]
	public void ReturnTrueIfPropertyFound()
	{
		// Act
		var value = _model.TrySetPropertyValue(nameof(TestClass.Value), NewValue);

		// Assert
		value
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFalseIfPropertyNotFound()
	{
		// Act
		var value = _model.TrySetPropertyValue("NotRealProperty", NewValue);

		// Assert
		value
			.Should()
			.BeFalse();
	}

	[Fact]
	public void SetValue()
	{
		// Act
		_model.TrySetPropertyValue(nameof(TestClass.Value), NewValue);

		// Assert
		_model
			.Value
			.Should()
			.Be(NewValue);
	}

	[Fact]
	public void ThrowExceptionIfPropertyTypeMismatched()
	{
		// Arrange
		var action = () => _model.SetPropertyValue(nameof(TestClass.Value), 1);

		// Assert
		action
			.Should()
			.ThrowExactly<InvalidCastException>();
	}
}
