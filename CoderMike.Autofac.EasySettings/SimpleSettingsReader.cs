using System;
using System.Collections.Specialized;
using System.IO;
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

		public SimpleSettingsReader(string filePath, SettingFileType settingFileType)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("Must specify a file", "filePath");
			}

			switch (settingFileType)
			{
				case SettingFileType.Json:
					_settingsProvider = new JsonSettingsProvider(filePath);
					break;
				case SettingFileType.Xml:
					_settingsProvider = new XmlSettingsProvider(filePath);
					break;
				default:
					throw new ArgumentOutOfRangeException("settingFileType",
						settingFileType,
						"Only JSON and XML file types are supported");
			}
		}

		public SimpleSettingsReader(string filePath)
			: this(filePath, DetectFileType(filePath))
		{
		}

		private static SettingFileType DetectFileType(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("Must specify a file", "filePath");
			}

			switch (Path.GetExtension(filePath).ToLowerInvariant())
			{
				case ".json":
					return SettingFileType.Json;
				case ".xml":
					return SettingFileType.Xml;
				default:
					throw new ArgumentException("Cannot determine the file type automatically.",
						"filePath");
			}
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