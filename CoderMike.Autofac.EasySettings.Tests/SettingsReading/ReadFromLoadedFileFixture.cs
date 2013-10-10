using System;
using System.Xml;

using Xunit;
using Xunit.Extensions;

namespace CoderMike.Autofac.EasySettings.Tests.SettingsReading
{
	public class ReadFromLoadedFileFixture
	{
		[Fact]
		public void CanLoadValuesFromAutodetectedJsonFile()
		{
			var reader = new SimpleSettingsReader("MultipleEntries.json");

			var testSettings = reader.Read<FirstSettings>();

			Assert.NotNull(testSettings);
		}

		[Fact]
		public void WillLoadConfigurationValuesFromAutodetectedJsonFile()
		{
			var reader = new SimpleSettingsReader("MultipleEntries.json");

			var testSettings = reader.Read<FirstSettings>();

			Assert.Equal("blah", testSettings.Value);
		}

		[Fact]
		public void CanLoadValuesFromAutodetectedXmlFile()
		{
			var reader = new SimpleSettingsReader("MultipleEntries.xml");

			var testSettings = reader.Read<FirstSettings>();

			Assert.NotNull(testSettings);
		}

		[Fact]
		public void WillLoadConfigurationValuesFromAutodetectedXmlFile()
		{
			var reader = new SimpleSettingsReader("MultipleEntries.xml");

			var testSettings = reader.Read<FirstSettings>();

			Assert.Equal("etc", testSettings.Value);
		}

		[Theory]
		[InlineData("MultipleEntriesXml")]
		[InlineData("UnsupportedType.txt")]
		public void WillThrowArgumentExceptionIfCannotAutodetectFileType(string filePath)
		{
			Assert.Throws<ArgumentException>(() => new SimpleSettingsReader(filePath));
		}

		[Fact]
		public void CanLoadValuesFromFileSpecifiedAsJson()
		{
			var reader = new SimpleSettingsReader("MultipleEntriesJson", SettingFileType.Json);

			var testSettings = reader.Read<FirstSettings>();

			Assert.NotNull(testSettings);
		}

		[Fact]
		public void WillLoadConfigurationValuesFromFileSpecifiedAsJson()
		{
			var reader = new SimpleSettingsReader("MultipleEntriesJson", SettingFileType.Json);

			var testSettings = reader.Read<FirstSettings>();

			Assert.Equal("blah", testSettings.Value);
		}

		[Fact]
		public void CanLoadValuesFromFileSpecifiedAsXml()
		{
			var reader = new SimpleSettingsReader("MultipleEntriesXml", SettingFileType.Xml);

			var testSettings = reader.Read<FirstSettings>();

			Assert.NotNull(testSettings);
		}

		[Fact]
		public void WillLoadConfigurationValuesFromFileSpecifiedAsXml()
		{
			var reader = new SimpleSettingsReader("MultipleEntriesXml", SettingFileType.Xml);

			var testSettings = reader.Read<FirstSettings>();

			Assert.Equal("etc", testSettings.Value);
		}

		[Fact]
		public void WillFailToParseConfigurationFileIfTypeIncorrect()
		{
			Assert.Throws<XmlException>(() => new SimpleSettingsReader("MultipleEntriesJson", SettingFileType.Xml));
		}

		[Fact]
		public void WillThrowOnUnknownSettingFileType()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => new SimpleSettingsReader("MultipleEntriesJson", (SettingFileType) 5));
		}
	}

	public class FirstSettings
	{
		public string Value { get; set; }
	}
}
