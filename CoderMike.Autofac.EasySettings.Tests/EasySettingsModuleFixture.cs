using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac;
using System.Collections.Specialized;

namespace CoderMike.Autofac.EasySettings.Tests
{
    [TestClass]
    public class EasySettingsModuleFixture
    {
        [TestMethod]
        public void ModuleInstallsSimpleSettingsReaderByDefault()
        {
            var settings = new NameValueCollection();
            var module = new EasySettingsModule(settings);

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            var container = builder.Build();
            var reader = container.Resolve<ISettingsReader>();

            Assert.IsNotNull(reader);
            Assert.IsTrue(reader is SimpleSettingsReader);
        }


        [TestMethod]
        public void ModuleInstallsSettingsSource()
        {
            var settings = new NameValueCollection();
            var module = new EasySettingsModule(settings);

            var builder = new ContainerBuilder();
            builder.RegisterModule(module);

            var container = builder.Build();
            Assert.IsTrue(container.ComponentRegistry.Sources.OfType<SettingsSource>().Any());
        }
    }
}
