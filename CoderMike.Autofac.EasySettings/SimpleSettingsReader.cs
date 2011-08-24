using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace CoderMike.Autofac.EasySettings
{
    public class SimpleSettingsReader : ISettingsReader
    {
        private readonly NameValueCollection _settingsSource;

        public SimpleSettingsReader(NameValueCollection settingsSource)
        {
            if (settingsSource == null)
                throw new ArgumentNullException("settingsSource");
            _settingsSource = settingsSource;
        }

        public object Read(Type settingsType)
        {
            if (settingsType == null)
                throw new ArgumentNullException("settingsType");
            var settingsObj = Activator.CreateInstance(settingsType);
            var settingsPrefix = settingsType.Name.Replace("Settings", "") + ":";
            foreach (var key in _settingsSource.AllKeys.Where(x => x.StartsWith(settingsPrefix)))
            {
                var propertyName = key.Substring(settingsPrefix.Length);
                var property = settingsType.GetProperty(propertyName);
                if (property == null)
                    throw new Exception(String.Format("Settings class {0} has no property called {1}", settingsType.Name, propertyName));

                var settingValue = Convert.ChangeType(_settingsSource[key], property.PropertyType);
                property.SetValue(settingsObj, settingValue, null);
            }
            return settingsObj;
        }
    }
}
