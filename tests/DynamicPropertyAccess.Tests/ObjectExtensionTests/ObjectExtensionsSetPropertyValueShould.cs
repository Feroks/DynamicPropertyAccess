using FluentAssertions;
using System;
using Xunit;

namespace DynamicPropertyAccess.Tests.ObjectExtensionTests;

public class ObjectExtensionsSetPropertyValueShould
{
	private const string? NewValue = "New Value";
	private readonly TestClass _model = new();

	[Fact]
	public void SetValue()
	{
		// Act
		_model.SetPropertyValue(nameof(TestClass.Value), NewValue);

		// Assert
		_model
			.Value
			.Should()
			.Be(NewValue);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFound()
	{
		// Arrange
		var action = () => _model.SetPropertyValue("NotRealProperty", NewValue);

		// Assert
		action
			.Should()
			.ThrowExactly<PropertyNotFoundException>();
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

	[Fact]
	public void ThrowExceptionIfPropertyDoesNotHaveSetter()
	{
		// Act
		var action = () => _model.SetPropertyValue(nameof(TestClass.PropertyWithoutSet), 1);

		// Assert
		action
			.Should()
			.ThrowExactly<PropertyDoesNotHaveSetterException>();
	}
}
