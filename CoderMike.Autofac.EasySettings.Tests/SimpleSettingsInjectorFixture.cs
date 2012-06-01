using System;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoderMike.Autofac.EasySettings.Tests
{
       [TestClass]
    public class SimpleSettingsInjectorFixture
    {
           [TestMethod]
           public void CanReadAStringValue()
           {
               var settingsSource = new NameValueCollection();
               settingsSource["My:Name"] = "Mike";
               var reader = new SimpleSettingsReader(settingsSource);
               var injector = new SimpleSettingsInjector();
               var settings = new MySettings();
               injector.Inject(settings, reader);
               Assert.AreEqual("Mike", settings.Name);
           }

        // TODO: Does this need a custom exception class?
        [TestMethod, ExpectedException(typeof(Exception))]
        public void FailsIfSettingsHaveNoCorrespondingProperty()
        {
            SimpleSettingsReader reader = null;
            SimpleSettingsInjector injector = null; 
            try
            {
                var settingsSource = new NameValueCollection();
                settingsSource["My:Name"] = "Mike";
                settingsSource["My:Blog"] = "http://codermike.com";
                reader = new SimpleSettingsReader(settingsSource);
                injector = new SimpleSettingsInjector();
            }
            catch (Exception ex)
            {
                Assert.Inconclusive("Unexpected Exception occurred: {0}", ex);
            }
            var settings = new MySettings();
            injector.Inject(settings, reader);
            Assert.Fail("Should have thrown");
        }

           [TestMethod]
           public void CanReadTypesOtherThanString()
           {
               var settingsSource = new NameValueCollection();
               settingsSource["Smtp:Server"] = "fake-server";
               settingsSource["Smtp:Port"] = "12345";
               settingsSource["Smtp:UseSSL"] = "True";
               var injector = new SimpleSettingsInjector();
               var reader = new SimpleSettingsReader(settingsSource);
               var settings = new SmtpSettings();
               injector.Inject(settings, reader);


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
               var injector = new SimpleSettingsInjector();
               var settings = new SmtpSettings();

               injector.Inject(settings, reader);
               Assert.Fail("Should have thrown");
           }

           [TestMethod]
           public void CanReadManySettingsFromASingleSource()
           {
               var settingsSource = new NameValueCollection();
               settingsSource["Smtp:Server"] = "test-server";
               settingsSource["My:Name"] = "Mike";
               var reader = new SimpleSettingsReader(settingsSource);
               var injector = new SimpleSettingsInjector();
               var smtpSettings = new SmtpSettings();
               var mySettings = new MySettings();

               injector.Inject(smtpSettings, reader);
               injector.Inject(mySettings, reader);

               Assert.AreEqual("test-server", smtpSettings.Server);
               Assert.AreEqual("Mike", mySettings.Name);
           }

           [TestMethod]
           public void CanSetPrivateProperties()
           {
               var settingsSource = new NameValueCollection();
               settingsSource["Foo:Bar"] = "Baz";
               var reader = new SimpleSettingsReader(settingsSource);
               var injector = new SimpleSettingsInjector();
               var settings = new FooSettings();

               injector.Inject(settings, reader);
               Assert.AreEqual("Baz", settings.Bar);
           }

           [TestMethod, ExpectedException(typeof(ArgumentException))]
           public void AttemptToSetTheUnsettableResultsInAnException()
           {
               var settingsSource = new NameValueCollection();
               settingsSource["Test:Unsettable"] = "Bang";
               var reader = new SimpleSettingsReader(settingsSource);
               var injector = new SimpleSettingsInjector();
               var settings = new TestSettings();

               injector.Inject(settings, reader);
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
}