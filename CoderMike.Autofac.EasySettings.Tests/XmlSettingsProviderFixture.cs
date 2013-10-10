using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoderMike.Autofac.EasySettings.Tests
{
	[TestClass]
	[DeploymentItem("SingleEntry.xml")]
	[DeploymentItem("MultipleEntries.xml")]
	public class XmlSettingsProviderFixture
	{
		[TestMethod]
		public void SingleEntryXmlHasExpectedNumberOfKeys()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			Assert.AreEqual(1, provider.AllKeys.Count());
		}

		[TestMethod]
		public void SingleEntryXmlHasExpectedKey()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			CollectionAssert.Contains(provider.AllKeys.ToList(), "TestValue");
		}

		[TestMethod]
		public void SingleEntryXmlHasExpectedValue()
		{
			var provider = new XmlSettingsProvider("SingleEntry.xml");

			Assert.AreEqual("5", provider["TestValue"]);
		}

		[TestMethod]
		public void MultipleEntriesXmlHasExpectedNumberOfKeys()
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.AreEqual(3, provider.AllKeys.Count());
		}

		[TestMethod]
		public void SingleEntriesXmlHasExpectedKeys()
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			CollectionAssert.AreEquivalent(provider.AllKeys.ToList(),
				new[] {"FirstValue", "SecondValue", "ThirdValue"});
		}

		[TestMethod]
		public void MultipleEntriesXmlHasExpectedValue()
		{
			var provider = new XmlSettingsProvider("MultipleEntries.xml");

			Assert.AreEqual("something", provider["ThirdValue"]);
		}
	}
}