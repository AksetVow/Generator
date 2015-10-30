﻿using Core.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Parser
{
    public static class ExportConfigParser
    {
        public const string ExportConfigurationNamePattern = @"^\[.*\]$";

        public const string Rootdir = @"rootdir";
        public const string Headertpl = @"headertpl";
        public const string Articletpl = @"articletpl";
        public const string Footertpl = @"footertpl";
        public const string IncludeToc = @"includetoc";
        public const string Tocheadertpl = @"tocheadertpl";
        public const string Toctpl = @"toctpl";
        public const string Tocfootertpl = @"tocfootertpl"; 


        public static IList<Template> ParseExportSettings(string exportSetting)
        {
            var templates = new List<Template>();

            StreamReader file = new StreamReader(exportSetting, Encoding.GetEncoding(1251));
            string line;
            Regex searchImportNameRegex = new Regex(ExportConfigurationNamePattern);
            Template template = null;

            while ((line = file.ReadLine()) != null)
            {
                var matchName = searchImportNameRegex.Match(line);

                if (matchName.Success)
                {
                    if (template != null)
                    {
                        templates.Add(template);
                    }

                    template = new Template();
                    template.Name = matchName.Value.Substring(1, matchName.Length - 2);
                }
                else
                {
                    if (template != null)
                    {
                        ParseConfiguration(line, template);
                    }
                }

            }

            if (!templates.Contains(template))
            {
                templates.Add(template);
            }

            return templates;
        }


        private static void ParseConfiguration(string line, Template template)
        {
            if (line.IndexOf(Rootdir) == 0)
            {
                template.Rootdir = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.IndexOf(Headertpl) == 0)
            {
                template.Headertpl = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.IndexOf(Articletpl) == 0)
            {
                template.Articletpl = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.IndexOf(Footertpl) == 0)
            {
                template.Footertpl = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.IndexOf(IncludeToc) == 0)
            {
                template.IncludeToc = line.Substring(line.IndexOf('=') + 1) == "1";
            }
            else if (line.IndexOf(Tocheadertpl) == 0)
            {
                template.Tocheadertpl = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.IndexOf(Toctpl) == 0)
            {
                template.Toctpl = line.Substring(line.IndexOf('=') + 1);
            }
            else if (line.IndexOf(Tocfootertpl) == 0)
            {
                template.Tocfootertpl = line.Substring(line.IndexOf('=') + 1);
            }
        }

        private static void ParseForImages(Template template)
        { 
            //TODO implement logic for parsing template
        }
    }
}
