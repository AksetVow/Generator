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
        private const string TestArticleWithMoreContent = @"TestData\11415944.htm";

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
        public void TestImportFile()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var article = _importer.ImportFile(TestArticle);

            Assert.IsTrue(article.Source.Equals("Continent"));
            Assert.IsTrue(article.Title.Equals("В центре Киева прошел ежегодный марафон"));
            Assert.IsTrue(article.Category.Equals("Интернет"));
            Assert.IsTrue(article.PublicDate.Equals("28.09.2015"));
        }

        [TestMethod]
        public void TestImportFileWithMoreContent()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var article = _importer.ImportFile(TestArticleWithMoreContent);

            Assert.IsTrue(article.Source.Equals( "FINBALANCE"));
            Assert.IsTrue(article.Title.Equals("Укргазбанк  виділяє кредит для ПАТ \"Аграрний фонд\" на 300 млн грн. під 25% річних "));
            Assert.IsTrue(article.Category.Equals("Интернет"));
            Assert.IsTrue(article.PublicDate.Equals("20.10.2015"));
        }

        [TestMethod]
        public void TestImportFiles()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var list = new List<string>();
            list.Add(TestArticle);
            list.Add(TestArticleWithMoreContent);

            var articles = _importer.ImportFiles(list);
            Assert.AreEqual(articles.Count, 2);

            Assert.IsTrue(articles[0].Source.Equals("Continent"));
            Assert.IsTrue(articles[0].Title.Equals("В центре Киева прошел ежегодный марафон"));
            Assert.IsTrue(articles[0].Category.Equals("Интернет"));
            Assert.IsTrue(articles[0].PublicDate.Equals("28.09.2015"));

            Assert.IsTrue(articles[1].Source.Equals("FINBALANCE"));
            Assert.IsTrue(articles[1].Title.Equals("Укргазбанк  виділяє кредит для ПАТ \"Аграрний фонд\" на 300 млн грн. під 25% річних "));
            Assert.IsTrue(articles[1].Category.Equals("Интернет"));
            Assert.IsTrue(articles[1].PublicDate.Equals("20.10.2015"));
        }


    }
}
