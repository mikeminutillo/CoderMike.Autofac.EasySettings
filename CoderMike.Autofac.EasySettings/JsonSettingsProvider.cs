using System;
using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

namespace CoderMike.Autofac.EasySettings
{
	public class JsonSettingsProvider : ISettingsProvider
	{
		private readonly Dictionary<string, string> _values;

		public JsonSettingsProvider(string filePath)
		{
			_values = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
		}

		public IEnumerable<string> AllKeys
		{
			get
			{
				return _values.Keys;
			}
		}

		public string this[string key]
		{
			get { return _values[key]; }
		}
	}
}