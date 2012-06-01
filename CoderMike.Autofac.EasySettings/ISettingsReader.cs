using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace CoderMike.Autofac.EasySettings
{
    public interface ISettingsReader
    {
        NameValueCollection Read();
    }
}
