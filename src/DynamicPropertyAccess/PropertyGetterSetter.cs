using System;

namespace DynamicPropertyAccess;

internal record PropertyGetterSetter(Lazy<Func<object, object?>> Getter, Lazy<Action<object, object?>> Setter)
{
	/// <summary>
	/// Instance of <see cref="PropertyGetterSetter"/> with getter and setter that do nothing.
	/// </summary>
	internal static readonly PropertyGetterSetter Empty = new(new(() => _ => default), new(() => (_, _) => { }));
}
