using Core.Export;
using Core.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Parser
{
    public static class ImportConfigParser
    {
        public const string ImportConfigurationNamePattern = @"^\[.*\]$";

        public const string Filemask = @"filemask";
        public const string Ziparchive = @"ziparchive";
        public const string Reseparator = @"reseparator";
        public const string Regetarticletext = @"regetarticletext";
        public const string Regetauthor = @"regetauthor";
        public const string Regetpublicdate = @"regetpublicdate";
        public const string Regetsourcenumber = @"regetsourcenumber";
        public const string Regetsource = @"regetsource";
        public const string Regettitle = @"regettitle";
        public const string Regetcategory = @"regetcategory";
        public const string Regetregion = @"regetregion";
        public const string Categoryempty = @"categoryempty";
        public const string Regetkeywords = @"regetkeywords";


        public static IList<ImportConfiguration> ParseImportSettings(string importSettingFile)
        {
            var importConfigurations = new List<ImportConfiguration>();

            StreamReader file = new StreamReader(importSettingFile, Encoding.GetEncoding(1251));
            string line;
            Regex searchImportNameRegex = new Regex(ImportConfigurationNamePattern);
            ImportConfiguration configuration = null;

            while ((line = file.ReadLine()) != null)
            {
                var matchName = searchImportNameRegex.Match(line);

                if (matchName.Success)
                {
                    if (configuration != null)
                    {
                        importConfigurations.Add(configuration);
                    }

                    configuration = new ImportConfiguration();
                    configuration.Name = matchName.Value.Substring(1, matchName.Length - 2);
                }
                else
                {
                    if (configuration != null)
                    {
                        ParseConfiguration(line, configuration);
                    }
                }

            }

            if (!importConfigurations.Contains(configuration))
            {
                importConfigurations.Add(configuration);
            }



            return importConfigurations;
        }

        private static void ParseConfiguration(string line, ImportConfiguration configuration)
        {
            if (line.Contains(Filemask))
            {
                configuration.FileMask = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Reseparator))
            {
                configuration.Reseparator = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetarticletext))
            {
                configuration.Regetarticletext = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetauthor))
            {
                configuration.Regetauthor = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetpublicdate))
            {
                configuration.Regetpublicdate = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetsourcenumber))
            {
                configuration.Regetsourcenumber = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetsource))
            {
                configuration.Regetsource = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regettitle))
            {
                configuration.Regettitle = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetcategory))
            {
                configuration.Regetcategory = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetcategory))
            {
                configuration.Regetcategory = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetregion))
            {
                configuration.Regetregion = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Categoryempty))
            {
                configuration.Categoryempty = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.Contains(Regetkeywords))
            {
                configuration.Regetkeywords = line.Substring(line.IndexOf('=') + 1);
            }

            else if (line.Contains(Ziparchive))
            {
                configuration.IsArchive = line.Substring(line.IndexOf('=') + 1) == "1";
            }

        }


    }
}
