using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Compression;
using System.Linq;

namespace Core.Import
{
    public class Importer
    {
        private const string Generator = "Generator";

        public ImportConfiguration ImportConfiguration { get; set; }        

        public IList<Article> Import(ImportData importData)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            if (ImportConfiguration.IsArchive)
            {
                return ImportArchives(importData.FilePathes);
            }
            else
            {
                return ImportFiles(importData.FilePathes);
            }
        }

        public IList<Article> ImportFiles(IEnumerable<string> files)
        {
            var result = new List<Article>();
            Article article;

            foreach (string file in files)
            {
                article = ImportFile(file);
                result.Add(article);
            }

            return result;

        }

        public IList<Article> ImportArchives(IEnumerable<string> archives)
        {
            var result = new List<Article>();
            IList<Article> articles;

            foreach (string archive in archives)
            { 
                articles = ImportArchive(archive);
                result.AddRange(articles);
            }

            return result;
        }

        public IList<Article> ImportArchive(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            string randomFolder = Guid.NewGuid().ToString();

            string destination = Path.Combine(Path.GetTempPath(), Generator);

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            destination = Path.Combine(destination, randomFolder);
            Directory.CreateDirectory(destination);


            ZipFile.ExtractToDirectory(filepath, destination);

            var files = Directory.GetFiles(destination).Where(f => !f.EndsWith("contents.htm"));

            return ImportFiles(files);

        }

        //TODO remove magic numbers in groups
        public Article ImportFile(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            string str = File.ReadAllText(filepath, Encoding.GetEncoding(1251));

            Article article = new Article();
            var matches = Regex.Matches(str, ImportConfiguration.Regetarticletext, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.ArticleText = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetauthor, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 2)
                article.Author = matches[0].Groups[2].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetpublicdate, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.PublicDate = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Categoryempty, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.CategoryEmpty = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetcategory, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.Category = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetkeywords, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.KeyWords = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetregion, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.Region = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetsource, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.Source = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetsourcenumber, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.SourceNumber = matches[0].Groups[1].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regettitle, RegexOptions.Singleline);
            if (matches.Count > 0 && matches[0].Groups.Count > 1)
                article.Title = matches[0].Groups[1].Value;

            ProcessImages(str, article);

            return article;
        }

        private void ProcessImages(string str, Article article)
        { 
        
        }


    }
}
