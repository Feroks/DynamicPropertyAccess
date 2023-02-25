using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using DynamicPropertyAccessor;
using FastMember;
using System.Reflection;

namespace DynamicPropertyAccess.Benchmarks;

[SimpleJob(RuntimeMoniker.Net70)]
public class SetterBenchmark
{
	private TestClass _instance = null!;
	private string _propertyName = null!;
	private string _value = null!;
	private Action<object, object?> _setter = null!;
	private TypeAccessor _accessor = null!;
	private PropertyInfo _propertyInfo = null!;

	[GlobalSetup]
	public void GlobalSetup()
	{
		_instance = new TestClass();
		_propertyName = nameof(TestClass.Value);
		_value = "MyValue";
		_setter = typeof(TestClass).GetPropertySetter(_propertyName);
		_accessor = TypeAccessor.Create(_instance.GetType());
		_propertyInfo = _instance.GetType().GetProperty(_propertyName)!;
	}

	[Benchmark]
	public void DynamicPropertyAccess()
	{
		_instance.SetPropertyValue(_propertyName, _value);
	}

	[Benchmark]
	public void DynamicPropertyAccessor()
	{
		_instance.SetProperty(_propertyName, _value);
	}

	[Benchmark]
	public void FastMember()
	{
		var accessor = TypeAccessor.Create(_instance.GetType());
		accessor[_instance, _propertyName] = _value;
	}

	[Benchmark]
	public void Reflection()
	{
		var propertyInfo = _instance.GetType().GetProperty(_propertyName);
		propertyInfo!.SetValue(_instance, _value);
	}

	[Benchmark]
	public void DynamicPropertyAccessReused()
	{
		_setter(_instance, _value);
	}

	[Benchmark]
	public void FastMemberReused()
	{
		_accessor[_instance, _propertyName] = _value;
	}

	[Benchmark]
	public void ReflectionReused()
	{
		_propertyInfo.SetValue(_instance, _value);
	}

	private class TestClass
	{
		public string? Value { get; set; }
	}
}
