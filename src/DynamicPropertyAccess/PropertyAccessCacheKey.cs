using System;

namespace DynamicPropertyAccess;

internal readonly struct PropertyAccessCacheKey : IEquatable<PropertyAccessCacheKey>
{
	internal PropertyAccessCacheKey(string propertyName, Type type)
	{
		PropertyName = propertyName;
		Type = type;
	}

	internal string PropertyName { get; }

	internal Type Type { get; }

	public bool Equals(PropertyAccessCacheKey other)
	{
		// Only check property name, because Type is used to avoid closure
		return PropertyName == other.PropertyName;
	}

	public override bool Equals(object? obj)
	{
		return obj is PropertyAccessCacheKey other && Equals(other);
	}

	public override int GetHashCode()
	{
		return PropertyName.GetHashCode();
	}
}
