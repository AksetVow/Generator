using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Core;
using Core.Parser;

namespace TestCore.Tests
{
    [TestClass]
    public class ImportParserTest
    {
        [TestMethod]
        public void TestParseImportSettings()
        {
            Assert.IsTrue(File.Exists(Settings.ImportIni));

            var importConfigurations = ImportConfigParser.ParseImportSettings(Settings.ImportIni);
            Assert.AreEqual(importConfigurations.Count, 4);

            Assert.AreEqual(importConfigurations[0].Name, "Отчеты Артефакта (отдельными файлами)");
            Assert.IsFalse(importConfigurations[0].IsArchive);
            Assert.AreEqual(importConfigurations[0].FileMask, "*.htm");

            Assert.AreEqual(importConfigurations[1].Name, "Отчеты Артефакта (архив ZIP)");
            Assert.IsTrue(importConfigurations[1].IsArchive);
            Assert.AreEqual(importConfigurations[1].FileMask, "*.zip");

            Assert.AreEqual(importConfigurations[2].Name, "Отчеты Артефакта-TV (архив ZIP)");
            Assert.IsTrue(importConfigurations[2].IsArchive);
            Assert.AreEqual(importConfigurations[2].FileMask, "*.zip");

            Assert.AreEqual(importConfigurations[3].Name, "Отчет-таблица (фрагменты статей)");
            Assert.IsTrue(importConfigurations[3].IsArchive);
            Assert.AreEqual(importConfigurations[3].FileMask, "*.zip");

        }
    }
}
