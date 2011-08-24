using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;

namespace CoderMike.Autofac.EasySettings.Tests
{
    [TestClass]
    public class SettingsSourceFixture
    {
        [TestMethod]
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
            Assert.AreSame(fakeSettings, resolvedSettings);
            Assert.IsTrue(reader.WasCalled);
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
