﻿using System;
using System.Collections.Specialized;

using CoderMike.Autofac.EasySettings;

using Xunit;

namespace CoderMike.EasySettings.Tests.SettingsReading
{
    public class ReadFromNameValueCollectionFixture
    {
        [Fact]
        public void RequiresASettingsSource()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleSettingsReader((NameValueCollection) null));
        }

        [Fact]
        public void CannotGetNullSettings()
        {
            var reader = new SimpleSettingsReader(new NameValueCollection());
            Assert.Throws<ArgumentNullException>(() => reader.Read(null));
        }

        [Fact]
        public void ShouldGetAnObjectEvenIfNoSettingsAreFound()
        {
            var reader = new SimpleSettingsReader(new NameValueCollection());
            var settings = reader.Read(typeof(MySettings));
            Assert.NotNull(settings);
	        Assert.IsType<MySettings>(settings);
        }

        [Fact]
        public void CanReadAStringValue()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["My:Name"] = "Mike";
            var reader = new SimpleSettingsReader(settingsSource);
            var settings = reader.Read<MySettings>();
            Assert.Equal("Mike", settings.Name);
        }

        // TODO: Does this need a custom exception class?
        [Fact]
        public void FailsIfSettingsHaveNoCorrespondingProperty()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["My:Name"] = "Mike";
            settingsSource["My:Blog"] = "http://codermike.com";
            var reader = new SimpleSettingsReader(settingsSource);
            Assert.Throws<Exception>(() => reader.Read<MySettings>());
        }

        [Fact]
        public void CanReadTypesOtherThanString()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Smtp:Server"] = "fake-server";
            settingsSource["Smtp:Port"] = "12345";
            settingsSource["Smtp:UseSSL"] = "True";
            var reader = new SimpleSettingsReader(settingsSource);
            var settings = reader.Read<SmtpSettings>();
            Assert.Equal("fake-server", settings.Server);
            Assert.Equal(12345, settings.Port);
            Assert.True(settings.UseSSL);
        }

        [Fact]
        public void UnconvertableSettingResultsInFormatException()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Smtp:Port"] = "abcdef";
            var reader = new SimpleSettingsReader(settingsSource);
            
			Assert.Throws<FormatException>(() => reader.Read<SmtpSettings>());
        }

        [Fact]
        public void CanReadManySettingsFromASingleSource()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Smtp:Server"] = "test-server";
            settingsSource["My:Name"] = "Mike";
            var reader = new SimpleSettingsReader(settingsSource);
            var smtpSettings = reader.Read<SmtpSettings>();
            var mySettings = reader.Read<MySettings>();
            Assert.Equal("test-server", smtpSettings.Server);
            Assert.Equal("Mike", mySettings.Name);
        }

        [Fact]
        public void CanSetPrivateProperties()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Foo:Bar"] = "Baz";
            var reader = new SimpleSettingsReader(settingsSource);
            var settings = reader.Read<FooSettings>();
            Assert.Equal("Baz", settings.Bar);
        }

        [Fact]
        public void AttemptToSetTheUnsettableResultsInAnException()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Test:Unsettable"] = "Bang";
            var reader = new SimpleSettingsReader(settingsSource);
            
			Assert.Throws<ArgumentException>(() => reader.Read<TestSettings>());
        }
    }

    class MySettings
    {
        public string Name { get; set; }
    }

    class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }

    class FooSettings
    {
        public string Bar { get; private set; }
    }

    class TestSettings
    {
        public string Unsettable
        {
            get { return "Calculated Value"; }
        }
    }
}