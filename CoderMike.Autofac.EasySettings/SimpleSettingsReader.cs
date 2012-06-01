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

     

        public NameValueCollection Read()
        {
            return _settingsSource;
        }
    }
}
