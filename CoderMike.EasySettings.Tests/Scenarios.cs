using System;
using System.Configuration;

using Autofac;

using CoderMike.Autofac.EasySettings;

using Xunit;

namespace CoderMike.EasySettings.Tests
{
    public class Scenarios
    {
        [Fact]
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

            Assert.Equal(2345, component.EmailSettings.Port);
            Assert.True(component.EmailSettings.Ssl);
            Assert.Equal("MyUserName", component.EmailSettings.Username);
            Assert.Equal("SomeKey", component.AkismetSettings.ApiKey);
            Assert.True(component.BlogSettings.EnableComments);
            Assert.True(component.BlogSettings.EnableHistory);
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
        }
    }
}
