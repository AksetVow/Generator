﻿using Core;
using Core.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestCore.Tests
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
            Assert.AreEqual(templates[0].Countheadertpl, @"..\cntheader.htm");
            Assert.AreEqual(templates[0].Counttpl, @"..\cnt.htm");
            Assert.AreEqual(templates[0].Countfootertpl, @"..\cntfooter.htm");
            Assert.AreEqual(templates[0].Countgrouptpl, @"..\countgrouptpl.htm");
            Assert.AreEqual(templates[0].Countindentstr, @"&nbsp;&nbsp;");
        }

        [TestMethod]
        //"Ignore due to old test structure"
        [Ignore]
        public void TestParseExportSettings_Old()
        {
            Assert.IsTrue(File.Exists("exports_old.ini"));

            var templates = ExportConfigParser.ParseExportSettings("exports_old.ini");
            Assert.AreEqual(templates.Count, 25);

            Assert.AreEqual(templates[0].Rootdir, @"outtpls\za-datu");
            Assert.AreEqual(templates[0].Headertpl, @"header.htm");
            Assert.AreEqual(templates[0].Articletpl, @"..\article.htm");
            Assert.AreEqual(templates[0].Footertpl, @"footer.htm");
            Assert.AreEqual(templates[0].IncludeToc, true);
            Assert.AreEqual(templates[0].Toctpl, @"..\toc.htm");
            Assert.AreEqual(templates[0].Tocheadertpl, @"..\tocheadertpl.htm");
            Assert.AreEqual(templates[0].Tocfootertpl, @"..\tocfootertpl.htm");
        }


    }
}
