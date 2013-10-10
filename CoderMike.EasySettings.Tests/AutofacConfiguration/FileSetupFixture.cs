using System;
using System.Linq;

using Autofac;

using CoderMike.Autofac.EasySettings;
using CoderMike.EasySettings.Tests.SettingsReading;

using Xunit;

namespace CoderMike.EasySettings.Tests.AutofacConfiguration
{
	public class FileSetupFixture
	{
		[Fact]
		public void ModuleInstallsSimpleSettingsReaderByDefault()
		{
			var module = new EasySettingsModule("MultipleEntries.json");

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
			var module = new EasySettingsModule("MultipleEntries.json");

			var builder = new ContainerBuilder();
			builder.RegisterModule(module);

			var container = builder.Build();
			Assert.NotEmpty(container.ComponentRegistry.Sources.OfType<SettingsSource>());
		}

		[Fact]
		public void ConfigurationValuesLoadedFromSuppliedCollection()
		{
			var module = new EasySettingsModule("MultipleEntries.json");

			var builder = new ContainerBuilder();
			builder.RegisterModule(module);

			var container = builder.Build();
			var reader = container.Resolve<ISettingsReader>();

			var instance = reader.Read<FirstSettings>();

			Assert.Equal("blah", instance.Value);
		}
	}
}