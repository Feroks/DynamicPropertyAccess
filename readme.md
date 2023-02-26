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
|       DynamicPropertyAccess |             .NET 7.0 |             .NET 7.0 |  26.776 ns | 0.4549 ns | 0.4255 ns |
|     DynamicPropertyAccessor |             .NET 7.0 |             .NET 7.0 |  31.530 ns | 0.3986 ns | 0.3728 ns |
|                  FastMember |             .NET 7.0 |             .NET 7.0 |  30.972 ns | 0.4472 ns | 0.4183 ns |
|                  Reflection |             .NET 7.0 |             .NET 7.0 |  33.527 ns | 0.4686 ns | 0.4383 ns |
| DynamicPropertyAccessReused |             .NET 7.0 |             .NET 7.0 |   1.475 ns | 0.0332 ns | 0.0311 ns |
|            FastMemberReused |             .NET 7.0 |             .NET 7.0 |  14.770 ns | 0.1585 ns | 0.1483 ns |
|            ReflectionReused |             .NET 7.0 |             .NET 7.0 |  12.105 ns | 0.2688 ns | 0.2514 ns |
|       DynamicPropertyAccess | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  66.566 ns | 1.2969 ns | 1.3877 ns |
|     DynamicPropertyAccessor | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  57.735 ns | 1.1492 ns | 1.4113 ns |
|                  FastMember | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  43.592 ns | 0.1853 ns | 0.1547 ns |
|                  Reflection | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 169.533 ns | 2.0884 ns | 1.6305 ns |
| DynamicPropertyAccessReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   5.731 ns | 0.0291 ns | 0.0227 ns |
|            FastMemberReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  24.150 ns | 0.1821 ns | 0.1703 ns |
|            ReflectionReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 120.565 ns | 2.4210 ns | 4.9455 ns |

As you can see, `GetPropertyValue` is slightly slower on .NET Framework than available alternatives.
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

