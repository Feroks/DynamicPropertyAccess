using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace DynamicPropertyAccess;

/// <summary>
/// Class that caches getters and setter for types.
/// </summary>
public static class PropertyAccessCache
{
	private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<PropertyAccessCacheKey, PropertyGetterSetter?>> Cache = new();
	private static readonly ParameterExpression ObjectParameter = Expression.Parameter(typeof(object), "source");
	private static readonly ParameterExpression PropertyValueParameter = Expression.Parameter(typeof(object), "value");
	private static readonly ParameterExpression[] GetterLambdaParameters = { ObjectParameter };
	private static readonly ParameterExpression[] SetterLambdaParameters = { ObjectParameter, PropertyValueParameter };

	/// <summary>
	/// Clear all cached data.
	/// </summary>
	public static void Clear()
	{
		Cache.Clear();
	}

	internal static bool TryGetObjectGetterSetter(Type type, string propertyName, out PropertyGetterSetter getterSetter)
	{
		var value = Cache
			.GetOrAdd(type, static _ => new())
			// Create key to avoid creating closure for "type" variable 
			.GetOrAdd(new PropertyAccessCacheKey(propertyName, type), x => CreateGetterSetter(x.Type, x.PropertyName));

		// Comparison with null is much faster than comparing to PropertyGetterSetter.Empty
		if (value != null)
		{
			getterSetter = value;
			return true;
		}

		getterSetter = PropertyGetterSetter.Empty;
		return false;
	}

	internal static PropertyGetterSetter GetObjectGetterSetter(Type type, string propertyName)
	{
		return TryGetObjectGetterSetter(type, propertyName, out var getterSetter)
			? getterSetter
			: throw new ArgumentException("Property not found on type", nameof(propertyName));
	}

	private static PropertyGetterSetter? CreateGetterSetter(Type type, string propertyName)
	{
		return type.GetProperty(propertyName) != null
			? new PropertyGetterSetter(
				new Lazy<Func<object, object?>>(() => CreateGetter(type, propertyName)),
				new Lazy<Action<object, object?>>(() => CreateSetter(type, propertyName)))
			: null;
	}

	private static Func<object, object?> CreateGetter(Type objectType, string propertyName)
	{
		var convert = Expression.Convert(ObjectParameter, objectType);
		var property = Expression.Property(convert, propertyName);
		var convertProperty = Expression.Convert(property, typeof(object));
		var lambda = Expression.Lambda<Func<object, object?>>(convertProperty, GetterLambdaParameters);

		return lambda.Compile();
	}

	private static Action<object, object?> CreateSetter(Type objectType, string propertyName)
	{
		var objectConvert = Expression.Convert(ObjectParameter, objectType);
		var property = Expression.Property(objectConvert, propertyName);

		var propertyValueConvert = Expression.Convert(PropertyValueParameter, property.Type);
		var assign = Expression.Assign(property, propertyValueConvert);
		var lambda = Expression.Lambda<Action<object, object?>>(assign, SetterLambdaParameters);

		return lambda.Compile();
	}
}
