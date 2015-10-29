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
        #region Constants
        private const string Generator = "Generator";
        private const string ImgRegex = "<img.+?src=[\"'](.+?)[\"'].*?>";
        public const int TextEncoding = 1251;
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
                return ImportFiles(importData.FilePathes);
            }
        }

        #region PrivateMethods
        private IList<Article> ImportFiles(IEnumerable<string> files)
        {
            var result = new List<Article>();
            Article article;

            var copies = new List<string>();
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

            foreach (var copy in copies)
            {
                article = ImportFile(copy);
                result.Add(article);
            }

            return result;
        }

        private IList<Article> ImportArchives(IEnumerable<string> archives)
        {
            var result = new List<Article>();
            IList<Article> articles;

            foreach (string archive in archives)
            { 
                articles = ImportArchive(archive);
                result.AddRange(articles);

                _currentDestination = CreateTemporaryFolder();
            }

            return result;
        }

        private IList<Article> ImportArchive(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            ZipFile.ExtractToDirectory(filepath, _currentDestination);

            var files = Directory.GetFiles(_currentDestination).Where(f => !f.EndsWith("contents.htm"));

            return ImportFiles(files);

        }

        private Article ImportFile(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            string str = File.ReadAllText(filepath, Encoding.GetEncoding(TextEncoding));

            Article article = new Article();
            article.Filepath = str;
            var matches = Regex.Matches(str, ImportConfiguration.Regetarticletext, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.ArticleText = matches[0].Value;

            matches = Regex.Matches(str, ImportConfiguration.Regetauthor, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Author = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetpublicdate, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.PublicDate = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Categoryempty, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.CategoryEmpty = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetcategory, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Category = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetkeywords, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.KeyWords = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetregion, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Region = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetsource, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Source = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regetsourcenumber, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.SourceNumber = SelectResultValue(matches[0]);

            matches = Regex.Matches(str, ImportConfiguration.Regettitle, RegexOptions.Singleline);
            if (matches.Count > 0)
                article.Title = SelectResultValue(matches[0]);

            ProcessImages(str, article);

            return article;
        }

        private void ProcessImages(string str, Article article)
        {
            var matches = Regex.Matches(str, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
            var images = new List<string>();
            string image;

            for (int i = 0; i < matches.Count; i ++)
            {
                image = SelectResultValue(matches[i]);
                if (image != null)
                {
                    images.Add(image);
                }
            }

            article.Images = images;
        }


        private string SelectResultValue(Match match)
        {
            if (match.Groups.Count > 1)
            {
                string result;
                for (int i = match.Groups.Count; i > 0; i--)
                { 
                    result = match.Groups[i].Value;
                    if (!string.IsNullOrEmpty(result))
                    {
                        return result;
                    }
                }
            }

            return null;
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
