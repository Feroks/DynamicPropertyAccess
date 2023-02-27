namespace DynamicPropertyAccess.Tests;

internal class TestClass
{
	public string? Value { get; set; }

	public bool? NullableBool { get; set; }

	public int PropertyWithoutSet { get; }

	public int PropertyWithoutGet
	{
		set { }
	}
}
