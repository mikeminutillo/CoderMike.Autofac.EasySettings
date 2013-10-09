using System;
using System.Collections.Generic;

namespace CoderMike.Autofac.EasySettings
{
	public interface ISettingsProvider
	{
		IEnumerable<string> AllKeys { get; }
		string this[string key] { get; }
	}
}