using System;

namespace DynamicPropertyAccess;

/// <summary>
/// Exception that is thrown when property does not have setter.
/// </summary>
public class PropertyDoesNotHaveSetterException : Exception
{
	public PropertyDoesNotHaveSetterException(Type type, string propertyName)
		: base("Property does not have setter")
	{
		Type = type;
		PropertyName = propertyName;
	}

	public Type Type { get; }

	public string PropertyName { get; }
}
