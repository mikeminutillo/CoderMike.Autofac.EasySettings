using System;
using System.Collections.Generic;

namespace CoderMike.EasySettings
{
	public interface ISettingsProvider
	{
		IEnumerable<string> AllKeys { get; }
		string this[string key] { get; }
	}
}