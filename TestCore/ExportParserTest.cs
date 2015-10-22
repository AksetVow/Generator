using Core;
using Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestCore
{
    [TestClass]
    public class ExportParserTest
    {
        [TestMethod]
        public void TestParseExportSettings()
        {
            Assert.IsTrue(File.Exists(Settings.ExportIni));

            var templates = ExportConfigParser.ParseExportSettings(Settings.ExportIni);
            Assert.AreEqual(templates.Count, 18);


        }

    }
}
