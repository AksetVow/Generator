﻿using Core.Export;
using Core.Import;
using Core.Parser;
using System.Collections.Generic;

namespace Core
{
    public static class Settings
    {
        private static List<Template> _templates = new List<Template>();
        private static List<ImportConfiguration> _importConfigurations = new List<ImportConfiguration>();

        public const string ExportIni = "exports.ini";
        public const string ImportIni = "imports.ini";

        static Settings()
        {
            var importConfigurations = ImportConfigParser.ParseImportSettings(ImportIni);
            _importConfigurations.AddRange(importConfigurations);

            var templates = ExportConfigParser.ParseExportSettings(ExportIni);
            _templates.AddRange(templates);
        }

        public static IEnumerable<Template> Templates
        { 
            get 
            {
                return _templates;
            }        
        }

        public static IEnumerable<ImportConfiguration> ImportConfigurations
        {
            get 
            {
                return _importConfigurations;
            }
        }

    }
}
