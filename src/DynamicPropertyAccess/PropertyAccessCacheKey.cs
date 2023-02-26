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
		return PropertyName == other.PropertyName && Type == other.Type;
	}

	public override bool Equals(object? obj)
	{
		return obj is PropertyAccessCacheKey other && Equals(other);
	}

	public override int GetHashCode()
	{
		unchecked
		{
			return (PropertyName.GetHashCode() * 397) ^ Type.GetHashCode();
		}
	}
}
