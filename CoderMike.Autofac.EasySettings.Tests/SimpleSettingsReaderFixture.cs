using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace CoderMike.Autofac.EasySettings.Tests
{
    [TestClass]
    public class SimpleSettingsReaderFixture
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void RequiresASettingsSource()
        {
            var reader = new SimpleSettingsReader(null);
        }
        
        [TestMethod]
        public void ShouldGetAnCollectionEvenIfNoSettingsAreFound()
        {
            var reader = new SimpleSettingsReader(new NameValueCollection());
            object settings = reader.Read();
            Assert.IsNotNull(settings, "Didn't get a settings object at all");
            Assert.IsNotNull(settings as NameValueCollection, "Didn't get a settings object of the correct type");
        }
        
  
    }
}
