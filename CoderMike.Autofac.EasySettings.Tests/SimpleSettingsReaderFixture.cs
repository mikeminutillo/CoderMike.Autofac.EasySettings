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
        public void CannotGetNullSettings()
        {
            var exceptionWasThrown = false;
            var reader = new SimpleSettingsReader(new NameValueCollection());
            try
            {
                reader.Read(null);
            }
            catch (ArgumentNullException)
            {
                exceptionWasThrown = true;
            }
            Assert.IsTrue(exceptionWasThrown, "Call should have thrown an exception");
        }
    }
}
