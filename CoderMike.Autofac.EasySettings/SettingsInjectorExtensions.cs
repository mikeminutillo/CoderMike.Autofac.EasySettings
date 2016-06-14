using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoderMike.Autofac.EasySettings
{
    public static class SettingsInjectorExtensions
    {
        public static void Inject(this ISettingsInjector injector, object instance, params ISettingsReader[] args)
        {
            injector.Inject(instance, args);
        }
    }
}
