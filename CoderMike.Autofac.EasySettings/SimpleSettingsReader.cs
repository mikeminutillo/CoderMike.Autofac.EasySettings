using System;
using System.Collections.Specialized;
using System.Linq;

namespace CoderMike.Autofac.EasySettings
{
	public class SimpleSettingsReader : ISettingsReader
	{
		private readonly ISettingsProvider _settingsProvider;

		public SimpleSettingsReader(NameValueCollection settingsSource)
		{
			if (settingsSource == null)
			{
				throw new ArgumentNullException("settingsSource");
			}
			_settingsProvider = new NameValueCollectionSettingsProvider(settingsSource);
		}

		public SimpleSettingsReader(ISettingsProvider settingsProvider)
		{
			if (settingsProvider == null)
			{
				throw new ArgumentNullException("settingsProvider");
			}

			_settingsProvider = settingsProvider;
		}

		public object Read(Type settingsType)
		{
			if (settingsType == null)
			{
				throw new ArgumentNullException("settingsType");
			}
			var settingsObj = Activator.CreateInstance(settingsType);
			var settingsPrefix = settingsType.Name.Replace("Settings", "") + ":";
			foreach (var key in _settingsProvider.AllKeys.Where(x => x.StartsWith(settingsPrefix)))
			{
				var propertyName = key.Substring(settingsPrefix.Length);
				var property = settingsType.GetProperty(propertyName);
				if (property == null)
				{
					throw new Exception(String.Format("Settings class {0} has no property called {1}", settingsType.Name, propertyName));
				}

				var settingValue = Convert.ChangeType(_settingsProvider[key], property.PropertyType);
				property.SetValue(settingsObj, settingValue, null);
			}
			return settingsObj;
		}
	}
}