using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Core.Parser;
using Core.Import;
using System.Collections.Generic;
using Core.Export;

namespace TestCore.Tests
{
    [TestClass]
    public class ExporterTest
    {
        private Importer _importer;
        private Exporter _exporter;
        private IList<ImportConfiguration> _importConfigurations;
        private IList<Template> _templates;
        private const string FileExtension = ".htm";

        [TestMethod]
        public void TestExport()
        {
            _importer.ImportConfiguration = _importConfigurations[1];

            var articles = _importer.Import(ImporterTest.TestArchive);
            Assert.AreEqual(articles.Count, 2);

            Workspace workspace = new Workspace();
            workspace.Add(articles);

            _exporter.Template = _templates[0];
            var result = _exporter.Export(workspace, Guid.NewGuid().ToString() + FileExtension, new UserRequestData());

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void TestExportWithPictures()
        {
            _importer.ImportConfiguration = _importConfigurations[1];

            var articles = _importer.Import(ImporterTest.TestLargeArchive);
            Assert.AreEqual(articles.Count, 712);

            Workspace workspace = new Workspace();
            workspace.Add(articles);

            _exporter.Template = _templates[0];
            var result = _exporter.Export(workspace, Guid.NewGuid().ToString() + FileExtension, new UserRequestData());

            Assert.IsNotNull(result);
        }

        [TestInitialize]
        public void Initialize()
        {
            _importer = new Importer();
            _exporter = new Exporter();
            _importConfigurations = ImportConfigParser.ParseImportSettings(Settings.ImportIni);
            _templates = ExportConfigParser.ParseExportSettings(Settings.ExportIni);
        }

    }
}
