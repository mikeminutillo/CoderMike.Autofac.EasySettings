using System;
using System.Collections.Specialized;
using System.Linq;

using Autofac;

using Xunit;

namespace CoderMike.Autofac.EasySettings.Tests
{
	public class EasySettingsModuleFixture
	{
		[Fact]
		public void ModuleInstallsSimpleSettingsReaderByDefault()
		{
			var settings = new NameValueCollection();
			var module = new EasySettingsModule(settings);

			var builder = new ContainerBuilder();
			builder.RegisterModule(module);

			var container = builder.Build();
			var reader = container.Resolve<ISettingsReader>();

			Assert.NotNull(reader);
			Assert.True(reader is SimpleSettingsReader);
		}

		[Fact]
		public void ModuleInstallsSettingsSource()
		{
			var settings = new NameValueCollection();
			var module = new EasySettingsModule(settings);

			var builder = new ContainerBuilder();
			builder.RegisterModule(module);

			var container = builder.Build();
			Assert.True(container.ComponentRegistry.Sources.OfType<SettingsSource>().Any());
		}
	}
}