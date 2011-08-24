using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoderMike.Autofac.EasySettings
{
    public static class SettingsReaderExtensions
    {
        public static T Read<T>(this ISettingsReader reader)
        {
            return (T)reader.Read(typeof(T));
        }
    }
}
