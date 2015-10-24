using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Import
{
    public class Importer
    {
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

        public IList<Article> ImportFiles(IList<string> files)
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

        public IList<Article> ImportArchives(IList<string> archives)
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

            throw new NotImplementedException();
        }


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
            if (matches.Count > 0)
                article.Author = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetpublicdate, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.PublicDate = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Categoryempty, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.CategoryEmpty = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetcategory, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Category = GetCategory(matches[0].Value);

            matches = Regex.Matches(str, ImportConfiguration.Regetkeywords, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.KeyWords = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetregion, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Region = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetsource, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Source = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetsourcenumber, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.SourceNumber = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regettitle, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Title = matches[0].Value;

            ProcessImages(str, article);

            return article;
        }

        private void ProcessImages(string str, Article article)
        { 
        
        }

        //TODO this implementation would depend on currently use regex
        private string GetCategory(string str)
        { 
            int beginIndex = str.IndexOf(':');
            int endIndex = str.IndexOf("-->");

            var result = str.Substring(beginIndex + 1, endIndex - beginIndex - 1);

            return result.Trim();
        }

        private string GetTitle(string str)
        {
            string result = "";

            return result;
        }


    }
}
