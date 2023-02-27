using System;

namespace DynamicPropertyAccess;

/// <summary>
/// Set of extensions for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
	private static readonly Func<object, object?> EmptyGetter = PropertyGetterSetter.Empty.Getter!;
	private static readonly Action<object, object?> EmptySetter = PropertyGetterSetter.Empty.Setter!;

	/// <summary>
	/// Get <see cref="Func{Object, Object}"/> that gets value of property called <see cref="propertyName"/> on passed object.
	/// </summary>
	/// <param name="source"><see cref="Type"/> containing property called <paramref name="propertyName"/>.</param>
	/// <param name="propertyName">Name fo the property.</param>
	/// <returns>Func that gets property value.</returns>
	/// <exception cref="PropertyDoesNotHaveGetterException">Thrown when property does not have getter.</exception>
	public static Func<object, object?> GetPropertyGetter(this Type source, string propertyName)
	{
		return PropertyAccessCache
			.GetObjectGetterSetter(source, propertyName)
			.Getter ?? throw new PropertyDoesNotHaveGetterException(source, propertyName);
	}

	/// <summary>
	/// Get <see cref="Func{Object, Object}"/> that gets value of property called <see cref="propertyName"/> on passed object.
	/// </summary>
	/// <param name="source"><see cref="Type"/> containing property called <paramref name="propertyName"/>.</param>
	/// <param name="propertyName">Name fo the property.</param>
	/// <param name="getter">Func that gets property value.</param>
	/// <returns>True, if <paramref name="propertyName"/> exists on <paramref name="source"/>.</returns>
	public static bool TryGetPropertyGetter(this Type source, string propertyName, out Func<object, object?> getter)
	{
		if (PropertyAccessCache.TryGetObjectGetterSetter(source, propertyName, out var getterSetter) && getterSetter.Getter != null)
		{
			getter = getterSetter.Getter;
			return true;
		}

		getter = EmptyGetter;
		return false;
	}

	/// <summary>
	/// Get <see cref="Func{Object, Object}"/> that sets value on property called <see cref="propertyName"/> on passed object.
	/// </summary>
	/// <param name="source"><see cref="Type"/> containing property called <paramref name="propertyName"/>.</param>
	/// <param name="propertyName">Name fo the property.</param>
	/// <returns>Action that sets property value.</returns>
	/// <exception cref="PropertyDoesNotHaveSetterException">Thrown when property does not have setter.</exception>
	public static Action<object, object?> GetPropertySetter(this Type source, string propertyName)
	{
		return PropertyAccessCache
			.GetObjectGetterSetter(source, propertyName)
			.Setter ?? throw new PropertyDoesNotHaveSetterException(source, propertyName);
	}

	/// <summary>
	/// Get <see cref="Func{Object, Object}"/> that sets value on property called <see cref="propertyName"/> on passed object.
	/// </summary>
	/// <param name="source"><see cref="Type"/> containing property called <paramref name="propertyName"/>.</param>
	/// <param name="propertyName">Name fo the property.</param>
	/// <param name="setter">Action that sets property value.</param>
	public static bool TryGetPropertySetter(this Type source, string propertyName, out Action<object, object?> setter)
	{
		if (PropertyAccessCache.TryGetObjectGetterSetter(source, propertyName, out var getterSetter) && getterSetter.Setter != null)
		{
			setter = getterSetter.Setter;
			return true;
		}

		setter = EmptySetter;
		return false;
	}
}
