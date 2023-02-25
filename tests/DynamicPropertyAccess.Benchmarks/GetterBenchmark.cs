using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using DynamicPropertyAccessor;
using FastMember;
using System.Reflection;

namespace DynamicPropertyAccess.Benchmarks;

[SimpleJob(RuntimeMoniker.Net70)]
[SimpleJob(RuntimeMoniker.Net472)]
public class GetterBenchmark
{
	private TestClass _instance = null!;
	private string _propertyName = null!;
	private Func<object, object?> _getter = null!;
	private TypeAccessor _accessor = null!;
	private PropertyInfo _propertyInfo = null!;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_instance = new TestClass();
		_propertyName = nameof(TestClass.Value);
		_getter = typeof(TestClass).GetPropertyGetter(_propertyName);
		_accessor = TypeAccessor.Create(_instance.GetType());
		_propertyInfo = _instance.GetType().GetProperty(_propertyName)!;
	}

	[Benchmark]
	public void DynamicPropertyAccess()
	{
		var _ = _instance.GetPropertyValue<string>(_propertyName);
	}

	[Benchmark]
	public void DynamicPropertyAccessor()
	{
		var _ = (string)_instance.GetProperty(_propertyName);
	}

	[Benchmark]
	public void FastMember()
	{
		var accessor = TypeAccessor.Create(_instance.GetType());
		var _ = accessor[_instance, _propertyName];
	}

	[Benchmark]
	public void Reflection()
	{
		var propertyInfo = _instance.GetType().GetProperty(_propertyName);
		var _ = propertyInfo!.GetValue(_instance);
	}

	[Benchmark]
	public void DynamicPropertyAccessReused()
	{
		var _ = (string?)_getter(_instance);
	}

	[Benchmark]
	public void FastMemberReused()
	{
		var _ = _accessor[_instance, _propertyName];
	}

	[Benchmark]
	public void ReflectionReused()
	{
		var _ = _propertyInfo.GetValue(_instance);
	}

	private class TestClass
	{
		public string? Value { get; set; }
	}
}
