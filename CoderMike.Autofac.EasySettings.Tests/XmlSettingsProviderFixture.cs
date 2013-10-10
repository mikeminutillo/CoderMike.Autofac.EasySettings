using System;
using System.Linq;

using Xunit;
using Xunit.Extensions;

namespace CoderMike.Autofac.EasySettings.Tests
{
	public class XmlSettingsProviderFixture
	{
		[Fact]
		public void SingleEntryXmlHasExpectedNumberOfKeys()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			Assert.Equal(1, provider.AllKeys.Count());
		}

		[Fact]
		public void SingleEntryXmlHasExpectedKey()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			Assert.Contains("Test:Value", provider.AllKeys);
		}

		[Fact]
		public void SingleEntryXmlHasExpectedValue()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			Assert.Equal("5", provider["Test:Value"]);
		}

		[Fact]
		public void MultipleEntriesXmlHasExpectedNumberOfKeys()
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.Equal(3, provider.AllKeys.Count());
		}

		[Theory]
		[InlineData("First:Value")]
		[InlineData("Second:Value")]
		[InlineData("Third:Value")]
		public void SingleEntriesXmlHasExpectedKeys(string key)
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.Contains(key, provider.AllKeys);
		}

		[Theory]
		[InlineData("First:Value", "etc")]
		[InlineData("Second:Value", "4")]
		[InlineData("Third:Value", "something")]
		public void MultipleEntriesXmlHasExpectedValue(string key, string value)
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.Equal(value, provider[key]);
		}
	}
}