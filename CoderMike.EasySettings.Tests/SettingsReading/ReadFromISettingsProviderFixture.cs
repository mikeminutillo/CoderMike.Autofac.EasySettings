using System;
using System.Collections.Generic;

using CoderMike.Autofac.EasySettings;

using Xunit;

namespace CoderMike.EasySettings.Tests.SettingsReading
{
	public class ReadFromISettingsProviderFixture
	{
		[Fact]
		public void RequiresASettingsSource()
		{
			Assert.Throws<ArgumentNullException>(() => new SimpleSettingsReader((ISettingsProvider) null));
		}

		[Fact]
		public void CannotGetNullSettings()
		{
			var reader = new SimpleSettingsReader(new TestSettingsProvider(new Dictionary<string, string>()));
			Assert.Throws<ArgumentNullException>(() => reader.Read(null));
		}

		[Fact]
		public void ShouldGetAnObjectEvenIfNoSettingsAreFound()
		{
			var reader = new SimpleSettingsReader(new TestSettingsProvider(new Dictionary<string, string>()));
			var settings = reader.Read(typeof(MySettings));
			Assert.NotNull(settings);
			Assert.IsType<MySettings>(settings);
		}

		[Fact]
		public void CanReadAStringValue()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"My:Name", "Mike"}
			});
			var reader = new SimpleSettingsReader(settingsProvider);
			var settings = reader.Read<MySettings>();
			Assert.Equal("Mike", settings.Name);
		}

		[Fact]
		public void FailsIfSettingsHaveNoCorrespondingProperty()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"My:Name", "Mike"},
				{"My:Blog", "http://codermike.com"}
			});
			var reader = new SimpleSettingsReader(settingsProvider);
			Assert.Throws<Exception>(() => reader.Read<MySettings>());
		}

		[Fact]
		public void CanReadTypesOtherThanString()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"Smtp:Server", "fake-server"},
				{"Smtp:Port", "12345"},
				{"Smtp:UseSSL", "True"}
			});
			var reader = new SimpleSettingsReader(settingsProvider);
			var settings = reader.Read<SmtpSettings>();
			Assert.Equal("fake-server", settings.Server);
			Assert.Equal(12345, settings.Port);
			Assert.True(settings.UseSSL);
		}

		[Fact]
		public void UnconvertableSettingResultsInFormatException()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"Smtp:Port", "abcdef"}
			});
			var reader = new SimpleSettingsReader(settingsProvider);

			Assert.Throws<FormatException>(() => reader.Read<SmtpSettings>());
		}

		[Fact]
		public void CanReadManySettingsFromASingleSource()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"Smtp:Server", "test-server"},
				{"My:Name", "Mike"}
			});

			var reader = new SimpleSettingsReader(settingsProvider);
			var smtpSettings = reader.Read<SmtpSettings>();
			var mySettings = reader.Read<MySettings>();
			Assert.Equal("test-server", smtpSettings.Server);
			Assert.Equal("Mike", mySettings.Name);
		}

		[Fact]
		public void CanSetPrivateProperties()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"Foo:Bar", "Baz"}
			});

			var reader = new SimpleSettingsReader(settingsProvider);
			var settings = reader.Read<FooSettings>();
			Assert.Equal("Baz", settings.Bar);
		}

		[Fact]
		public void AttemptToSetTheUnsettableResultsInAnException()
		{
			var settingsProvider = new TestSettingsProvider(new Dictionary<string, string>
			{
				{"Test:Unsettable", "Bang"}
			});

			var reader = new SimpleSettingsReader(settingsProvider);

			Assert.Throws<ArgumentException>(() => reader.Read<TestSettings>());
		}
	}

	public class TestSettingsProvider : ISettingsProvider
	{
		private readonly IDictionary<string, string> _values;

		public TestSettingsProvider(IDictionary<string, string> values)
		{
			_values = values;
		}

		public IEnumerable<string> AllKeys
		{
			get { return _values.Keys; }
		}

		public string this[string key]
		{
			get { return _values[key]; }
		}
	}
}