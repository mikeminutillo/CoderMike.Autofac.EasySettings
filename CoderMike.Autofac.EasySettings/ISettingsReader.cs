using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoderMike.Autofac.EasySettings
{
    public interface ISettingsReader
    {
        object Read(Type settingsType);
    }
}
