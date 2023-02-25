[![Nuget](https://img.shields.io/nuget/v/DynamicPropertyAccess)](https://www.nuget.org/packages/DynamicPropertyAccess/)

This library allows you to get or set values to properties by name. It uses compiled lambdas instead that are cached of reflection and is thread safe.

## Getting Property Value
You can get property value by calling:
```csharp
T ObjectExtensions.GetPropertyValue<T>(this object source, string propertyName)
```

Alternatively, you can create `Func` that gets property value from an object by calling:
```csharp
Func<object, object?> TypeExtensions.GetPropertyGetter(this Type type, string propertyName)
```

## Setting Property Value
You can set property value by calling:
```csharp
void ObjectExtensions.SetPropertyValue(this object source, string propertyName, object value)
```

Alternatively, you can create `Func` that sets property value on an object by calling:
```csharp
Action<object, object?> TypeExtensions.GetPropertySetter(this Type type, string propertyName)
```
Both methods have `TryGet` pattern alternatives.

## Benchmarks
You can see benchmark for `GetPropertyValue` method for this library, [DynamicPropertyAccessor
](https://github.com/mdavisJr/DynamicPropertyAccessor), [FastMember](https://github.com/mgravell/fast-member) and Reflection.  
Methods with `...Reused` suffix create `Func`/`PropertyInfo` once and then reuse it in benchmark.

``` ini
BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19044.2604/21H2/November2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
  [Host]               : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256
  .NET 7.0             : .NET 7.0.3 (7.0.323.6910), X64 RyuJIT AVX2
  .NET Framework 4.7.2 : .NET Framework 4.8 (4.8.4614.0), X64 RyuJIT VectorSize=256
```
|                      Method |                  Job |              Runtime |       Mean |     Error |    StdDev |
|---------------------------- |--------------------- |--------------------- |-----------:|----------:|----------:|
|       DynamicPropertyAccess |             .NET 7.0 |             .NET 7.0 |  38.063 ns | 0.7854 ns | 1.0484 ns |
|     DynamicPropertyAccessor |             .NET 7.0 |             .NET 7.0 |  31.728 ns | 0.4040 ns | 0.3779 ns |
|                  FastMember |             .NET 7.0 |             .NET 7.0 |  30.952 ns | 0.3137 ns | 0.2934 ns |
|                  Reflection |             .NET 7.0 |             .NET 7.0 |  33.789 ns | 0.3169 ns | 0.2964 ns |
| DynamicPropertyAccessReused |             .NET 7.0 |             .NET 7.0 |   1.677 ns | 0.0222 ns | 0.0208 ns |
|            FastMemberReused |             .NET 7.0 |             .NET 7.0 |  15.371 ns | 0.1688 ns | 0.1579 ns |
|            ReflectionReused |             .NET 7.0 |             .NET 7.0 |  12.011 ns | 0.1298 ns | 0.1215 ns |
|       DynamicPropertyAccess | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  88.398 ns | 0.6464 ns | 0.5397 ns |
|     DynamicPropertyAccessor | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  56.928 ns | 1.1533 ns | 1.0788 ns |
|                  FastMember | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  48.617 ns | 0.9610 ns | 1.0282 ns |
|                  Reflection | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 176.842 ns | 3.3967 ns | 4.0436 ns |
| DynamicPropertyAccessReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   6.066 ns | 0.0731 ns | 0.0684 ns |
|            FastMemberReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  29.359 ns | 0.3300 ns | 0.2926 ns |
|            ReflectionReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 121.696 ns | 2.4447 ns | 2.7173 ns |

As you can see, `GetPropertyValue` is slightly slower than available alternatives.
This is because of thread safety overhead. On the other hand, reused `Func` from `GetPropertyGetter` is significantly faster. 
Additionally, Reflection is almost as fast on .NET 7+.
You can read more about it [here](https://devblogs.microsoft.com/dotnet/announcing-dotnet-7-preview-5/#system-reflection-performance-improvements-when-invoking-members).

### Conclusions
- Use this library (and reuse Getter/Setter) if:
    - Read/Write operations are frequently performed.
    - Thread safety is important.
- Use other libraries if:
   - Read/Write operations are rarely performed.
   - Thread safety is not a concern.
   - Target framework is .NET Framework, .NET 6 or below.
- Use Reflection if:
   - Target framework is .NET 7+ and Read/Write operations are rarely performed.

