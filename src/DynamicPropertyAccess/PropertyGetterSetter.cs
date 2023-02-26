using System;

namespace DynamicPropertyAccess;

internal record PropertyGetterSetter(Func<object, object?> Getter, Action<object, object?> Setter)
{
	/// <summary>
	/// Instance of <see cref="PropertyGetterSetter"/> with getter and setter that do nothing.
	/// </summary>
	internal static readonly PropertyGetterSetter Empty = new(static _ => default, static (_, _) => { });
}
