using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CoderMike.Autofac.EasySettings
{
	public class XmlSettingsProvider : ISettingsProvider
	{
		private readonly Dictionary<string, string> _values;

		public XmlSettingsProvider(string filePath)
		{
			var document = XDocument.Load(filePath);

			if (document.Root == null)
			{
				throw new ArgumentException("The specified file could not be loaded.");
			}
			_values = document.Root.Elements().ToDictionary(element => element.Attribute("key").Value,
				element => element.Value);
		}

		public IEnumerable<string> AllKeys
		{
			get { return _values.Keys; }
		}

		public string this[string key]
		{
			get { return _values[key]; }
		}
	}
}