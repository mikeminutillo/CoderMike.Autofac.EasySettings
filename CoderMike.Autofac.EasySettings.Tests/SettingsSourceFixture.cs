using System;

using Autofac;

using Xunit;

namespace CoderMike.Autofac.EasySettings.Tests
{
    public class SettingsSourceFixture
    {
        [Fact]
        public void SettingsSourceAllowsResolvingOfSettingClasses()
        {
            var builder = new ContainerBuilder();
            var reader = new FakeReader();
            builder.RegisterInstance(reader).As<ISettingsReader>();
            builder.RegisterSource(new SettingsSource());

            var container = builder.Build();
            var fakeSettings = new FakeSettings();
            reader.Settings = fakeSettings;

            var resolvedSettings = container.Resolve<FakeSettings>();
            Assert.Same(fakeSettings, resolvedSettings);
            Assert.True(reader.WasCalled);
        }

        class FakeSettings
        {

        }


        class FakeReader : ISettingsReader
        {
            public object Settings { get; set; }
            public bool WasCalled { get; set; }

            public object Read(Type settingsType)
            {
                WasCalled = true;
                return Settings;
            }
        }
    }
}
