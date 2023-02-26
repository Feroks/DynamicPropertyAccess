using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using DynamicNotifyPropertyChanged;

namespace DynamicPropertyAccess.Benchmarks;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net472)]
public class CacheBenchmark
{
	private Random _random = null!;
	private Type _type = null!;
	private string _propertyName = null!;

	[Params(5, 10, 100, 1000)]
	public int TypeCount { get; set; }

	[Params(3, 10, 100)]
	public int PropertyCount { get; set; }

	[GlobalSetup]
	public void GlobalSetup()
	{
		_random = new Random();
	}

	[IterationSetup]
	public void IterationSetup()
	{
		PropertyAccessCache.Clear();

		var types = Enumerable
			.Range(0, TypeCount)
			.Select(_ => DynamicNotifyPropertyChangedClassFactory.CreateType(Enumerable
				.Range(0, PropertyCount)
				.Select((x, i) => new DynamicProperty($"Property{i}", typeof(string)))
				.ToArray()))
			.ToArray();

		_type = types[_random.Next(0, TypeCount)];
		_propertyName = $"Property{_random.Next(0, PropertyCount)}";
	}

	[Benchmark]
	public void Individual()
	{
		PropertyAccessCache.TryGetObjectGetterSetter(_type, _propertyName, out _);
	}
}
