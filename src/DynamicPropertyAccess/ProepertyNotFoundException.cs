using System;

namespace DynamicPropertyAccess;

/// <summary>
/// Exception that is thrown when property is not found on specified type.
/// </summary>
public class PropertyNotFoundException : Exception
{
	public PropertyNotFoundException(Type type, string propertyName)
		: base("Property was not found on specified type")
	{
		Type = type;
		PropertyName = propertyName;
	}

	public Type Type { get; }

	public string PropertyName { get; }
}
