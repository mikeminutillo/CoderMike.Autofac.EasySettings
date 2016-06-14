using System;
using System.Collections.Generic;
using System.Linq;

namespace CoderMike.Autofac.EasySettings
{
    public class SimpleSettingsInjector : ISettingsInjector
    {
        public void Inject(object settingsObj, IEnumerable<ISettingsReader> settings)
        {
            if (settingsObj == null)
                throw new ArgumentNullException("instance");

            var settingsType = settingsObj.GetType();
            var settingsPrefix = settingsType.Name.Replace("Settings", "") + ":";

            foreach (var settingsReader in settings)
            {
                var settingsSource = settingsReader.Read();
                if (settingsSource != null)
                {
                    foreach (var key in settingsSource.AllKeys.Where(x => x.StartsWith(settingsPrefix)))
                    {
                        var propertyName = key.Substring(settingsPrefix.Length);
                        var property = settingsType.GetProperty(propertyName);
                        if (property == null)
                            throw new Exception(String.Format("Settings class {0} has no property called {1}", settingsType.Name, propertyName));

                        var settingValue = Convert.ChangeType(settingsSource[key], property.PropertyType);
                        property.SetValue(settingsObj, settingValue, null);
                    }
                }
            }
        }
    }
}