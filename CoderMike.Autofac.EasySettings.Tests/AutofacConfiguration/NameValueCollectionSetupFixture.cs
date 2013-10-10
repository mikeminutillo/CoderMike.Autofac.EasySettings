using System;
using System.Collections.Specialized;
using System.Linq;

using Autofac;

using CoderMike.Autofac.EasySettings.Tests.SettingsReading;

using Xunit;

namespace CoderMike.Autofac.EasySettings.Tests.AutofacConfiguration
{
	public class NameValueCollectionSetupFixture
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
			Assert.IsType<SimpleSettingsReader>(reader);
		}

		[Fact]
		public void ModuleInstallsSettingsSource()
		{
			var settings = new NameValueCollection();
			var module = new EasySettingsModule(settings);

			var builder = new ContainerBuilder();
			builder.RegisterModule(module);

			var container = builder.Build();
			Assert.NotEmpty(container.ComponentRegistry.Sources.OfType<SettingsSource>());
		}

		[Fact]
		public void ConfigurationValuesLoadedFromSuppliedCollection()
		{
			var settings = new NameValueCollection
			{
				{ "First:Value", "something"}
			};
			var module = new EasySettingsModule(settings);

			var builder = new ContainerBuilder();
			builder.RegisterModule(module);

			var container = builder.Build();
			var reader = container.Resolve<ISettingsReader>();

			var instance = reader.Read<FirstSettings>();

			Assert.Equal("something", instance.Value);
		}
	}
}