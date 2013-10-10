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

			Assert.Contains("Test:Value", provider.AllKeys);
		}

		[Fact]
		public void SingleEntryJsonHasExpectedValue()
		{
			var provider = new JsonSettingsProvider("SingleEntry.json");

			Assert.Equal("5", provider["Test:Value"]);
		}

		[Fact]
		public void MultipleEntriesJsonHasExpectedNumberOfKeys()
		{
			var provider = new JsonSettingsProvider("MultipleEntries.json");

			Assert.Equal(3, provider.AllKeys.Count());
		}

		[Theory]
		[InlineData("First:Value")]
		[InlineData("Second:Value")]
		[InlineData("Third:Value")]
		public void SingleEntriesJsonHasExpectedKeys(string key)
		{
			var provider = new JsonSettingsProvider("MultipleEntries.json");

			Assert.Contains(key, provider.AllKeys);
		}

		[Theory]
		[InlineData("First:Value", "blah")]
		[InlineData("Second:Value", "4")]
		[InlineData("Third:Value", "something")]
		public void MultipleEntriesJsonHasExpectedValue(string key, string value)
		{
			var provider = new JsonSettingsProvider("MultipleEntries.json");

			Assert.Equal(value, provider[key]);
		}
	}
}