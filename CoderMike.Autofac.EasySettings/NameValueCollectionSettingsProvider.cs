using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CoderMike.Autofac.EasySettings
{
	class NameValueCollectionSettingsProvider : ISettingsProvider
	{
		private readonly NameValueCollection _nameValueCollection;

		public NameValueCollectionSettingsProvider(NameValueCollection nameValueCollection)
		{
			_nameValueCollection = nameValueCollection;
		}

		public IEnumerable<string> AllKeys
		{
			get { return _nameValueCollection.AllKeys; }
		}

		public string this[string key]
		{
			get { return _nameValueCollection[key]; }
		}
	}
}