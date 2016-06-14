using System;
using System.Collections.Specialized;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using System.Configuration;

namespace CoderMike.Autofac.EasySettings.Tests
{
    [TestClass]
    public class Scenarios
    {
        [TestMethod]
        public void TestMethod1()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new EasySettingsModule(ConfigurationManager.AppSettings));
            builder.RegisterType<FakeComponent>();
            var container = builder.Build();

            var component = container.Resolve<FakeComponent>();

            var emailSettings = container.Resolve<EmailSettings>();
            var akismetSettings = container.Resolve<AkismetSettings>();
            var blogSettings = container.Resolve<BlogSettings>();

            Assert.AreEqual(2345, component.EmailSettings.Port);
            Assert.IsTrue(component.EmailSettings.Ssl);
            Assert.AreEqual("MyUserName", component.EmailSettings.Username);
            Assert.AreEqual("SomeKey", component.AkismetSettings.ApiKey);
            Assert.IsTrue(component.BlogSettings.EnableComments);
            Assert.IsTrue(component.BlogSettings.EnableHistory);
        }

        [TestMethod]
        public void MultipleSettingsReaders()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new EasySettingsModule(ConfigurationManager.AppSettings));
            //extra settings provider
            var secretItems = new NameValueCollection {{"Blog:SuperSecretItem", "SuperSecretValue"}};
            builder.RegisterInstance(new SimpleSettingsReader(secretItems)).As<ISettingsReader>();

            builder.RegisterType<FakeComponent>();

            var container = builder.Build();
            var component = container.Resolve<FakeComponent>();

            Assert.AreEqual("SuperSecretValue", component.BlogSettings.SuperSecretItem);
        }

        class FakeComponent
        {
            public EmailSettings EmailSettings { get; private set; }
            public AkismetSettings AkismetSettings { get; private set; }
            public BlogSettings BlogSettings { get; private set; }

            public FakeComponent(EmailSettings emailSettings, AkismetSettings akismetSettings, BlogSettings blogSettings)
            {
                EmailSettings = emailSettings;
                AkismetSettings = akismetSettings;
                BlogSettings = blogSettings;
            }
        }

        class EmailSettings
        {
            public int Port { get; private set; }
            public bool Ssl { get; private set; }
            public string Username { get; private set; }
        }

        class AkismetSettings
        {
            public string ApiKey { get; private set; }
        }

        class BlogSettings
        {
            public bool EnableComments { get; private set; }
            public bool EnableHistory { get; private set; }
            public string SuperSecretItem { get; private set; }
        }
    }
}
