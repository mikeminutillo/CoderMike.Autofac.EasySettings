using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace CoderMike.Autofac.EasySettings.Tests
{
    [TestClass]
    public class SimpleSettingsReaderFixture
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void RequiresASettingsSource()
        {
            var reader = new SimpleSettingsReader(null);
        }

        [TestMethod]
        public void CannotGetNullSettings()
        {
            var exceptionWasThrown = false;
            var reader = new SimpleSettingsReader(new NameValueCollection());
            try
            {
                reader.Read(null);
            }
            catch (ArgumentNullException)
            {
                exceptionWasThrown = true;
            }
            Assert.IsTrue(exceptionWasThrown, "Call should have thrown an exception");
        }

        [TestMethod]
        public void ShouldGetAnObjectEvenIfNoSettingsAreFound()
        {
            var reader = new SimpleSettingsReader(new NameValueCollection());
            object settings = reader.Read(typeof(MySettings));
            Assert.IsNotNull(settings, "Didn't get a settings object at all");
            Assert.IsNotNull(settings as MySettings, "Didn't get a settings object of the correct type");
        }

        [TestMethod]
        public void CanReadAStringValue()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["My:Name"] = "Mike";
            var reader = new SimpleSettingsReader(settingsSource);
            var settings = reader.Read<MySettings>();
            Assert.AreEqual("Mike", settings.Name);
        }

        // TODO: Does this need a custom exception class?
        [TestMethod, ExpectedException(typeof(Exception))]
        public void FailsIfSettingsHaveNoCorrespondingProperty()
        {
            SimpleSettingsReader reader = null;
            try
            {
                var settingsSource = new NameValueCollection();
                settingsSource["My:Name"] = "Mike";
                settingsSource["My:Blog"] = "http://codermike.com";
                reader = new SimpleSettingsReader(settingsSource);
            }
            catch (Exception ex)
            {
                Assert.Inconclusive("Unexpected Exception occurred: {0}", ex);
            }
            var settings = reader.Read<MySettings>();
            Assert.Fail("Should have thrown");
        }

        [TestMethod]
        public void CanReadTypesOtherThanString()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Smtp:Server"] = "fake-server";
            settingsSource["Smtp:Port"] = "12345";
            settingsSource["Smtp:UseSSL"] = "True";
            var reader = new SimpleSettingsReader(settingsSource);
            var settings = reader.Read<SmtpSettings>();
            Assert.AreEqual("fake-server", settings.Server);
            Assert.AreEqual(12345, settings.Port);
            Assert.IsTrue(settings.UseSSL);
        }

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void UnconvertableSettingResultsInFormatException()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Smtp:Port"] = "abcdef";
            var reader = new SimpleSettingsReader(settingsSource);
            var settings = reader.Read<SmtpSettings>();
            Assert.Fail("Should have thrown");
        }

        [TestMethod]
        public void CanReadManySettingsFromASingleSource()
        {
            var settingsSource = new NameValueCollection();
            settingsSource["Smtp:Server"] = "test-server";
            settingsSource["My:Name"] = "Mike";
            var reader = new SimpleSettingsReader(settingsSource);
            var smtpSettings = reader.Read<SmtpSettings>();
            var mySettings = reader.Read<MySettings>();
            Assert.AreEqual("test-server", smtpSettings.Server);
            Assert.AreEqual("Mike", mySettings.Name);
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
    }
}
