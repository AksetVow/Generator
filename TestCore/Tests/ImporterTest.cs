using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Import;
using System.Collections.Generic;
using Core.Parser;
using Core;

namespace TestCore.Tests
{
    [TestClass]
    public class ImporterTest
    {
        private const string TestArchive = @"TestData\test.zip";
        private const string TestArchiveWithoutContentsFile = @"TestData\test2.zip";
        private const string TestArticle = @"TestData\10990313.htm";

        private Importer _importer;
        private IList<ImportConfiguration> _importConfigurations;

        [TestInitialize]
        public void Initialize()
        {
            _importer = new Importer();
            _importConfigurations = ImportConfigParser.ParseImportSettings(Settings.ImportIni);
        }


        [TestMethod]
        public void TestImportArchive()
        {
            List<string> list = new List<string>();
            list.Add(TestArchive);
            ImportData data = new ImportData(new List<string>(list));

            _importer.ImportConfiguration = _importConfigurations[1];
            var articles = _importer.Import(data);

            Assert.AreEqual(articles.Count, 2);
            
        }


        [TestMethod]
        public void TestImportFewFiles()
        {



        }


        [TestMethod]
        public void TestImportFile()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var article = _importer.ImportFile(TestArticle);

            Assert.AreNotEqual(article.Source, "Continent");
            Assert.AreNotEqual(article.Title, "В центре Киева прошел ежегодный марафон");
            Assert.AreNotEqual(article.Category, "Интернет");
            Assert.AreNotEqual(article.PublicDate, "28.09.2015");


        }


    }
}
