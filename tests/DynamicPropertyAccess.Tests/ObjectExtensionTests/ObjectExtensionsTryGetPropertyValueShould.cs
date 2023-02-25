using FluentAssertions;
using System;
using Xunit;

namespace DynamicPropertyAccess.Tests.ObjectExtensionTests;

public class ObjectExtensionsTryGetPropertyValueShould
{
	private readonly TestClass _model = new()
	{
		Value = "My Value",
		NullableBool = false
	};

	[Fact]
	public void ReturnTrueIfPropertyFound()
	{
		// Act
		var value = _model.TryGetPropertyValue<string?>(nameof(TestClass.Value), out _);

		// Assert
		value
			.Should()
			.BeTrue();
	}

	[Fact]
	public void ReturnFalseIfPropertyNotFound()
	{
		// Act
		var value = _model.TryGetPropertyValue<string>("NotRealProperty", out _);

		// Assert
		value
			.Should()
			.BeFalse();
	}

	[Fact]
	public void GetValueGeneric()
	{
		// Act
		_model.TryGetPropertyValue<string>(nameof(TestClass.Value), out var value);

		// Assert
		value
			.Should()
			.Be(_model.Value);
	}

	[Fact]
	public void CastValueToObject()
	{
		// Act
		_model.TryGetPropertyValue<bool?>(nameof(TestClass.NullableBool), out var value);

		// Assert
		value
			.Should()
			.Be(_model.NullableBool);
	}

	[Fact]
	public void ThrowExceptionIfPropertyTypeMismatched()
	{
		// Arrange
		var func = () => _model.TryGetPropertyValue<int>(nameof(TestClass.Value), out var value);

		// Assert
		func
			.Should()
			.ThrowExactly<InvalidCastException>();
	}
}
