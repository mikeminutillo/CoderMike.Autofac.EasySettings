using System;
using System.Collections.Specialized;
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
            var injector = new FakeInjector();
            builder.RegisterInstance(reader).As<ISettingsReader>();
            builder.RegisterInstance(injector).As<ISettingsInjector>();
            builder.RegisterSource(new SettingsSource());

            var container = builder.Build();
            var fakeSettings = new NameValueCollection();
            reader.Collection = fakeSettings;

            var resolvedSettings = container.Resolve<FakeSettings>();
            
            Assert.IsNotNull(resolvedSettings);
            Assert.IsTrue(injector.WasCalled);
            Assert.IsNotNull(injector.Settings, "Settings sources were not passed to injector");
            Assert.IsTrue(injector.Settings.Contains(reader));
        }

        class FakeSettings
        {

        }


        class FakeReader : ISettingsReader
        {
            public NameValueCollection Collection { get; set; }
            public bool WasCalled { get; set; }

            public NameValueCollection Read()
            {
                WasCalled = true;
                return Collection;
            }
        }

        private class FakeInjector : ISettingsInjector
        {
            public bool WasCalled { get; set; }
            public IEnumerable<ISettingsReader> Settings { get; set; }

            public void Inject(object o, IEnumerable<ISettingsReader> settings)
            {
                WasCalled = true;
                Settings = settings;
            }

        }
    }
}
