using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using System.Collections.Specialized;

namespace CoderMike.Autofac.EasySettings
{
    public class EasySettingsModule : Module
    {
        private readonly NameValueCollection _settings;

        public EasySettingsModule(NameValueCollection settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleSettingsReader>()
                .As<ISettingsReader>()
                .WithParameter(TypedParameter.From(_settings));

            builder.RegisterSource(new SettingsSource());
        }
    }
}
