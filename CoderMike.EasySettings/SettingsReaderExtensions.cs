using System;

namespace CoderMike.EasySettings
{
    public static class SettingsReaderExtensions
    {
        public static T Read<T>(this ISettingsReader reader)
        {
            return (T)reader.Read(typeof(T));
        }
    }
}
