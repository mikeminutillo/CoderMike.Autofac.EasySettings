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

			Assert.Contains("TestValue", provider.AllKeys);
		}

		[Fact]
		public void SingleEntryXmlHasExpectedValue()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			Assert.Equal("5", provider["TestValue"]);
		}

		[Fact]
		public void MultipleEntriesXmlHasExpectedNumberOfKeys()
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.Equal(3, provider.AllKeys.Count());
		}

		[Theory]
		[InlineData("FirstValue")]
		[InlineData("SecondValue")]
		[InlineData("ThirdValue")]
		public void SingleEntriesXmlHasExpectedKeys(string key)
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.Contains(key, provider.AllKeys);
		}

		[Fact]
		public void MultipleEntriesXmlHasExpectedValue()
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.Equal("something", provider["ThirdValue"]);
		}
	}
}