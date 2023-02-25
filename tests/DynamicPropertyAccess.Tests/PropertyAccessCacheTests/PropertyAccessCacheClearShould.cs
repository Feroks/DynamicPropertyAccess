using FluentAssertions;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace DynamicPropertyAccess.Tests.PropertyAccessCacheTests;

// Run in sequence because it clears cache
[Collection("Sequential")]
public class PropertyAccessCacheClearShould
{
	[Fact]
	public void EmptyCachedGetterSetters()
	{
		// Arrange
		PropertyAccessCache.GetObjectGetterSetter(typeof(TestClass), nameof(TestClass.Value));
		var field = typeof(PropertyAccessCache).GetField("Cache", BindingFlags.Static | BindingFlags.NonPublic);
		var innerCache = field!.GetValue(null);
		var countProperty = innerCache!.GetType().GetProperty(nameof(ICollection<bool>.Count), BindingFlags.Instance | BindingFlags.Public);

		// Act
		PropertyAccessCache.Clear();

		// Assert
		var count = (int)countProperty!.GetValue(innerCache)!;

		count
			.Should()
			.Be(0);
	}
}
