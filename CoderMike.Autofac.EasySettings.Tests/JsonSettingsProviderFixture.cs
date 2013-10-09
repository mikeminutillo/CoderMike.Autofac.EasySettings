using System;
using System.Linq;

using Xunit;
using Xunit.Extensions;

namespace CoderMike.Autofac.EasySettings.Tests
{
	public class JsonSettingsProviderFixture
	{
		[Fact]
		public void SingleEntryJsonHasExpectedNumberOfKeys()
		{
			var provider = new JsonSettingsProvider("SingleEntry.json");

			Assert.Equal(1, provider.AllKeys.Count());
		}

		[Fact]
		public void SingleEntryJsonHasExpectedKey()
		{
			var provider = new JsonSettingsProvider("SingleEntry.json");

			Assert.Contains("TestValue", provider.AllKeys);
		}

		[Fact]
		public void SingleEntryJsonHasExpectedValue()
		{
			var provider = new JsonSettingsProvider("SingleEntry.json");

			Assert.Equal("5", provider["TestValue"]);
		}

		[Fact]
		public void MultipleEntriesJsonHasExpectedNumberOfKeys()
		{
			var provider = new JsonSettingsProvider("MultipleEntries.json");

			Assert.Equal(3, provider.AllKeys.Count());
		}

		[Theory]
		[InlineData("FirstValue")]
		[InlineData("SecondValue")]
		[InlineData("ThirdValue")]
		public void SingleEntriesJsonHasExpectedKeys(string key)
		{
			var provider = new JsonSettingsProvider("MultipleEntries.json");

			Assert.Contains(key, provider.AllKeys);
		}

		[Fact]
		public void MultipleEntriesJsonHasExpectedValue()
		{
			var provider = new JsonSettingsProvider("MultipleEntries.json");

			Assert.Equal("something", provider["ThirdValue"]);
		}
	}
}