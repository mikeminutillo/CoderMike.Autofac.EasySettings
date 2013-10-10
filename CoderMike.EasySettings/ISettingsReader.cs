using System;

namespace CoderMike.EasySettings
{
    public interface ISettingsReader
    {
        object Read(Type settingsType);
    }
}
