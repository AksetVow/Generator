using Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Import
{
    public class Importer
    {
        #region Constants
        private const string Generator = "Generator";
        public const string ImageRegex = "<img.+?src=[\"'](.+?)[\"'].*?>";
        public const int TextEncoding = 1251;
        private const string ArticleTextBegin = "<p class=\"documenttext\">";
        #endregion

        private string _baseDestination;
        private string _currentDestination;

        public ImportConfiguration ImportConfiguration { get; set; }

        public Importer()
        {
            _baseDestination = Path.Combine(Path.GetTempPath(), Generator);

            if (!Directory.Exists(_baseDestination))
            {
                Directory.CreateDirectory(_baseDestination);
            }

            _baseDestination = CreateTemporaryFolder();
        }

        public IList<Article> Import(params string[] files)
        {
            ImportData importData = new ImportData(files);

            var result = Import(importData);

            return result;
        }

        public IList<Article> Import(ImportData importData)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            _currentDestination = CreateTemporaryFolder();


            if (ImportConfiguration.IsArchive)
            {
                return ImportArchives(importData.FilePathes);
            }
            else
            {
                return ImportFiles(importData.FilePathes, true);
            }
        }

        #region PrivateMethods
        private IList<Article> ImportFiles(IEnumerable<string> files, bool copyToTemp)
        {
            var result = new List<Article>();
            Article article;

            IEnumerable<string> resultFiles;
            var copies = new List<string>();

            if (copyToTemp)
            {
                string newFilePath;

                foreach (string file in files)
                {
                    newFilePath = Path.Combine(_currentDestination, Path.GetFileName(file));
                    if (File.Exists(newFilePath))
                    {
                        newFilePath = Path.Combine(_currentDestination, Guid.NewGuid().ToString() + Path.GetExtension(file));
                    }
                    File.Copy(file, newFilePath);
                    copies.Add(newFilePath);
                }

                resultFiles = copies;
            }
            else
            {
                resultFiles = files;
            }

            foreach (var file in resultFiles)
            {
                article = ImportFile(file);
                result.Add(article);
            }

            return result;
        }

        private IList<Article> ImportArchives(IList<string> archives)
        {
            var result = new List<Article>();
            IList<Article> articles;

            for (int i = 0; i < archives.Count; i ++ )
            {
                articles = ImportArchive(archives[i]);
                result.AddRange(articles);

                if (i != archives.Count - 1)
                {
                    _currentDestination = CreateTemporaryFolder();
                }
            }

            return result;
        }

        private IList<Article> ImportArchive(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            ZipFile.ExtractToDirectory(filepath, _currentDestination);

            var files = Directory.GetFiles(_currentDestination).Where(f => !f.EndsWith("contents.htm"));

            return ImportFiles(files, false);

        }

        private Article ImportFile(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            string str = File.ReadAllText(filepath, Encoding.GetEncoding(TextEncoding));

            Article article = new Article();
            article.Filepath = filepath;
            var matches = Regex.Matches(str, ImportConfiguration.Regetarticletext, RegexOptions.Singleline);
            if (matches.Count > 0)
            {
                var articleText  = MatchHelper.SelectResultValue(matches[0]);
                articleText = ImportArticleText(articleText);
                article.ArticleText = articleText;
            }

            matches = Regex.Matches(str, ImportConfiguration.Regetauthor, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Author = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetpublicdate, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.PublicDate = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Categoryempty, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.CategoryEmpty = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetcategory, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Category = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetkeywords, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.KeyWords = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetregion, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Region = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetsource, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Source = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetsourcenumber, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.SourceNumber = MatchHelper.SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regettitle, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Title = MatchHelper.SelectResultValue(matches[0]).Trim();

            ProcessImages(str, article);

            return article;
        }

        private string ImportArticleText(string str)
        {
            int indexBegin = str.IndexOf(ArticleTextBegin);
            if (indexBegin != -1)
            {
                return str.Substring(indexBegin);
            }
            else
            {
                return str;
            }
        }

        private void ProcessImages(string str, Article article)
        {
            var matches = Regex.Matches(str, ImageRegex, RegexOptions.IgnoreCase);
            var images = new List<string>();
            MatchHelper.ProcessImages(matches, images);
            article.Images = images;
        }

        private string CreateTemporaryFolder()
        {
            string randomFolder = Guid.NewGuid().ToString();
            string tempFolder = Path.Combine(_baseDestination, randomFolder);
            Directory.CreateDirectory(tempFolder);

            return tempFolder;
        }

        #endregion

    }
}
