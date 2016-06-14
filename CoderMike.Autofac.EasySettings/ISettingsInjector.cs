using System.Collections.Generic;

namespace CoderMike.Autofac.EasySettings
{
    public interface ISettingsInjector
    {
        void Inject(object o, IEnumerable<ISettingsReader> settings);
    }
}