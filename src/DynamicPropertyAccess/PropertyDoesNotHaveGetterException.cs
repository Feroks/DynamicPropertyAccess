using System;

namespace DynamicPropertyAccess;

/// <summary>
/// Exception that is thrown when property does not have getter.
/// </summary>
public class PropertyDoesNotHaveGetterException : Exception
{
	public PropertyDoesNotHaveGetterException(Type type, string propertyName)
		: base("Property does not have getter")
	{
		Type = type;
		PropertyName = propertyName;
	}

	public Type Type { get; }

	public string PropertyName { get; }
}
