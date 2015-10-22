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

            Assert.AreEqual(templates[0].Rootdir, @"outtpls\strax");
            Assert.AreEqual(templates[0].Headertpl, @"header.htm");
            Assert.AreEqual(templates[0].Articletpl, @"article.htm");
            Assert.AreEqual(templates[0].Footertpl, @"footer.htm");
            Assert.AreEqual(templates[0].IncludeToc, true);
            Assert.AreEqual(templates[0].Toctpl, @"..\toc.htm");
            Assert.AreEqual(templates[0].Tocheadertpl, @"tocheadertpl.htm");
            Assert.AreEqual(templates[0].Tocfootertpl, @"..\tocfootertpl.htm");
        }

    }
}
