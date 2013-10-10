using System;
using System.Collections.Specialized;

using Autofac;

namespace CoderMike.Autofac.EasySettings
{
	public class EasySettingsModule : Module
	{
		private readonly Action<ContainerBuilder> _registrationAction;

		public EasySettingsModule(NameValueCollection settings)
		{
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}

			_registrationAction = builder => builder.RegisterType<SimpleSettingsReader>()
				.As<ISettingsReader>()
				.WithParameter(TypedParameter.From(settings));
		}

		public EasySettingsModule(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("A configuration file must be supplied.",
					"filePath");
			}
			_registrationAction = builder => builder.RegisterType<SimpleSettingsReader>()
				.As<ISettingsReader>()
				.WithParameter(TypedParameter.From(filePath));
		}

		public EasySettingsModule(ISettingsProvider settingsProvider)
		{
			if (settingsProvider == null)
			{
				throw new ArgumentNullException("settingsProvider");
			}

			_registrationAction = builder => builder.RegisterType<SimpleSettingsReader>()
				.As<ISettingsReader>()
				.WithParameter(TypedParameter.From(settingsProvider));
		}

		protected override void Load(ContainerBuilder builder)
		{
			_registrationAction(builder);

			builder.RegisterSource(new SettingsSource());
		}
	}
}