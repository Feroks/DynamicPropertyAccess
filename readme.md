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
|       DynamicPropertyAccess |             .NET 7.0 |             .NET 7.0 |  27.549 ns | 0.2752 ns | 0.2574 ns |
|     DynamicPropertyAccessor |             .NET 7.0 |             .NET 7.0 |  32.110 ns | 0.4228 ns | 0.3955 ns |
|                  FastMember |             .NET 7.0 |             .NET 7.0 |  31.624 ns | 0.3068 ns | 0.2870 ns |
|                  Reflection |             .NET 7.0 |             .NET 7.0 |  33.822 ns | 0.2084 ns | 0.1740 ns |
| DynamicPropertyAccessReused |             .NET 7.0 |             .NET 7.0 |   1.675 ns | 0.0204 ns | 0.0191 ns |
|            FastMemberReused |             .NET 7.0 |             .NET 7.0 |  15.345 ns | 0.1183 ns | 0.1106 ns |
|            ReflectionReused |             .NET 7.0 |             .NET 7.0 |  12.033 ns | 0.2232 ns | 0.2088 ns |
|       DynamicPropertyAccess | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  74.231 ns | 1.2512 ns | 1.1704 ns |
|     DynamicPropertyAccessor | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  57.478 ns | 1.0758 ns | 1.0063 ns |
|                  FastMember | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  44.485 ns | 0.1956 ns | 0.1734 ns |
|                  Reflection | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 177.810 ns | 1.8636 ns | 1.6520 ns |
| DynamicPropertyAccessReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 |   6.054 ns | 0.0508 ns | 0.0475 ns |
|            FastMemberReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 |  23.822 ns | 0.2128 ns | 0.1990 ns |
|            ReflectionReused | .NET Framework 4.7.2 | .NET Framework 4.7.2 | 120.524 ns | 1.5174 ns | 1.2671 ns |

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

