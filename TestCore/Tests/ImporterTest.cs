﻿using System;
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
        private const string TestLargeArchive = @"TestData\test3.zip";
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
            _importer.ImportConfiguration = _importConfigurations[1];
            var articles = _importer.ImportArchive(TestArchive);

            Assert.AreEqual(articles.Count, 2);

            TestKievArticle(articles[0]);
        }

        [TestMethod]
        public void TestImportFile()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var article = _importer.ImportFile(TestArticle);

            TestKievArticle(article, false);
        }

        [TestMethod]
        public void TestImportFileWithMoreContent()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var article = _importer.ImportFile(TestArticleWithMoreContent);

            TestUkrGazBankArticle(article, false);
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

            TestKievArticle(articles[0], false);
            TestUkrGazBankArticle(articles[1], false);
        }

        [TestMethod]
        public void TestImportArchives()
        {
            _importer.ImportConfiguration = _importConfigurations[1];
            var list = new List<string>();
            list.Add(TestArchive);
            list.Add(TestArchiveWithoutContentsFile);

            var articles = _importer.ImportArchives(list);
            Assert.AreEqual(articles.Count, 4);

            TestKievArticle(articles[0]);
            TestKievArticle(articles[2]);
        }



        [TestMethod]
        public void TestImportForFiles()
        {
            _importer.ImportConfiguration = _importConfigurations[0];
            var list = new List<string>();
            list.Add(TestArticle);
            list.Add(TestArticleWithMoreContent);
            ImportData importData = new ImportData(list);


            var articles = _importer.Import(importData);
            Assert.AreEqual(articles.Count, 2);
            TestKievArticle(articles[0], false);
            TestUkrGazBankArticle(articles[1], false);
        }

        [TestMethod]
        public void TestImportForArchives()
        {
            _importer.ImportConfiguration = _importConfigurations[1];
            var list = new List<string>();
            list.Add(TestArchive);
            list.Add(TestArchiveWithoutContentsFile);

            ImportData importData = new ImportData(list);

            var articles = _importer.Import(importData);
            Assert.AreEqual(articles.Count, 4);

            TestKievArticle(articles[0]);
            TestKievArticle(articles[2]);
        }

        [TestMethod]
        public void TestImportLargeArchive()
        {
            _importer.ImportConfiguration = _importConfigurations[1];
            var list = new List<string>();
            list.Add(TestLargeArchive);

            ImportData importData = new ImportData(list);

            var articles = _importer.Import(importData);
            Assert.AreEqual(articles.Count, 712);
        }


        #region PrivateMethodsForTestingSpecificArticlesInformation
        private void TestKievArticle(Article article, bool full = true)
        {
            Assert.IsTrue(article.Source.Equals("Continent"));
            Assert.IsTrue(article.Title.Equals("В центре Киева прошел ежегодный марафон"));
            Assert.IsTrue(article.Category.Equals("Интернет"));


            if (full)
            {
                Assert.IsTrue(article.PublicDate.Equals("28.09.2015 11:58"));
                Assert.IsTrue(article.KeyWords.Equals("ВДНХ"));
                Assert.IsTrue(article.Author.Equals("http://continent-news.info/newsinfo.html?unitId=941808"));
            }
            else
            {
                Assert.IsTrue(article.PublicDate.Equals("28.09.2015"));
            }
        
        }

        private void TestUkrGazBankArticle(Article article, bool full = true)
        {
            Assert.IsTrue(article.Title.Equals("Укргазбанк  виділяє кредит для ПАТ \"Аграрний фонд\" на 300 млн грн. під 25% річних "));
            Assert.IsTrue(article.Category.Equals("Интернет"));

            if (full)
            {
                Assert.IsTrue(article.Source.Equals("Экономические известия (www.eizvestia.com)"));
                Assert.IsTrue(article.PublicDate.Equals("28.09.2015 11:24"));
                Assert.IsTrue(article.KeyWords.Equals("ВДНХ"));
                Assert.IsTrue(article.Author.Equals("http://m.eizvestia.com/m?cat=news_kiev&url=153-v-centre-kieva-proshel-ezhegodnyj-marafon"));
            }
            else
            {
                Assert.IsTrue(article.Source.Equals("FINBALANCE"));
                Assert.IsTrue(article.PublicDate.Equals("20.10.2015"));
            }

        }

        #endregion


    }
}
