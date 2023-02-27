using FluentAssertions;
using System;
using Xunit;

namespace DynamicPropertyAccess.Tests.ObjectExtensionTests;

public class ObjectExtensionsGetPropertyValueShould
{
	private readonly TestClass _model = new()
	{
		Value = "My Value",
		NullableBool = false
	};

	[Fact]
	public void GetValueGeneric()
	{
		// Act
		var value = _model.GetPropertyValue<string>(nameof(TestClass.Value));

		// Assert
		value
			.Should()
			.Be(_model.Value);
	}

	[Fact]
	public void CastValueToObject()
	{
		// Act
		var value = _model.GetPropertyValue<bool?>(nameof(TestClass.NullableBool));

		// Assert
		value
			.Should()
			.Be(_model.NullableBool);
	}

	[Fact]
	public void ThrowExceptionIfPropertyNotFound()
	{
		// Arrange
		var func = () => _model.GetPropertyValue<string>("NotRealProperty");

		// Assert
		func
			.Should()
			.ThrowExactly<PropertyNotFoundException>();
	}

	[Fact]
	public void ThrowExceptionIfPropertyTypeMismatched()
	{
		// Arrange
		var func = () => _model.GetPropertyValue<int>(nameof(TestClass.Value));

		// Assert
		func
			.Should()
			.ThrowExactly<InvalidCastException>();
	}

	[Fact]
	public void ThrowExceptionIfPropertyDoesNotHaveGetter()
	{
		// Act
		var func = () =>  _model.GetPropertyValue<int>(nameof(TestClass.PropertyWithoutGet));

		// Assert
		func
			.Should()
			.ThrowExactly<PropertyDoesNotHaveGetterException>();
	}
}
