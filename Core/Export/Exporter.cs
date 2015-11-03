using Core.Import;
using System;
using System.IO;
using System.Text;

namespace Core.Export
{
    public class Exporter
    {
        public Template Template { get; set; }

        private Workspace _currentWorkspace;
        private ExportCounterSettings _counterSettings;
        private UserRequestData _userRequestData;
        private string _resultPath;

        private const string ArticleIndex = @"%ARTICLEINDEX%";
        private const string ArticleText = @"%ARTICLETEXT%";
        private const string Author = @"%AUTHOR%";
        private const string PublicDate = @"%PUBLICDATE%";
        private const string Source = @"%SOURCE%";
        private const string SourceNumber = @"%SOURCENUMBER%";
        private const string Title = @"%TITLE%";

        private const string CountList = @"%CNTLIST%";
        private const string Rating = @"%RATINGS%";
        private const string AlsoIn = @"%ALSOIN%";

        private const string Http = "http";


        public Report Export(Workspace workspace, string resultPath, UserRequestData userRequestData, ExportCounterSettings counterSettings = null)
        {
            _currentWorkspace = workspace;
            _counterSettings = counterSettings;
            _userRequestData = userRequestData;
            _resultPath = resultPath;

            string result = string.Empty;
            
            result += CreateHeader();

            if (Template.IncludeToc)
            {
                result += CreateTocHeader();
                result += CreateToc();
                result += CreateTocFooter();
            }

            result += CreateContent();

            result += CreateFooter();

            result = ExportTemplateImages(result);

            var report = CreateReport(result, resultPath);

            return report;
        }


        //TODO Change access to private
        #region PrivateMethods

        #region TOC
        private string CreateTocItem(Article article, string index)
        {
            string path = Path.Combine(Template.Rootdir, Template.Toctpl);
            string tocText = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            tocText = tocText.Replace(ArticleIndex, index);
            tocText = tocText.Replace(Title, article.Title);
            tocText = tocText.Replace(Source, article.Source);
            tocText = tocText.Replace(PublicDate, string.Empty);
            tocText = tocText.Replace(SourceNumber, article.SourceNumber);
            

            return tocText;
        }

        private string CreateToc()
        {
            string result = string.Empty;
            for (int i = 0; i < _currentWorkspace.Articles.Count; i++)
            {
                result += CreateTocItem(_currentWorkspace.Articles[i], (i + 1).ToString());
            }

            return result;
        }

        private string CreateTocHeader()
        {
            string path = Path.Combine(Template.Rootdir, Template.Tocheadertpl);
            string tocHeader = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            return tocHeader;
        }

        private string CreateTocFooter()
        {
            string path = Path.Combine(Template.Rootdir, Template.Tocfootertpl);
            string tocFooter = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            return tocFooter;
        }

        #endregion 

        #region Counter

        private string CreateCountTable()
        {
            throw new NotImplementedException();
        }

        private string CreateCountTableHeader()
        {
            throw new NotImplementedException();
        }

        private string CreateCountTableFooter()
        {
            throw new NotImplementedException();
        }
        

        #endregion

        #region Content
        private string CreateHeader()
        {
            string path = Path.Combine(Template.Rootdir, Template.Headertpl);
            string header = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            header = header.Replace(UserRequestData.ReportNameRegex, _userRequestData.ReportName);
            header = header.Replace(UserRequestData.ReportMaterialsRegex, _userRequestData.ReportMaterials);
            header = header.Replace(UserRequestData.ReportStartDateRegex, _userRequestData.ReportStartDateString);
            header = header.Replace(UserRequestData.ReportEndDateRegex, _userRequestData.ReportEndDateString);

            return header;
        }

        private string CreateFooter()
        {
            string path = Path.Combine(Template.Rootdir, Template.Footertpl);
            string footer = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            if (_counterSettings == null)
            {
                footer = footer.Replace(CountList, string.Empty);
                footer = footer.Replace(Rating, string.Empty);
            }
            else
            { 
            
            }

            return footer;
        }

        private string CreateContent()
        {
            string result = string.Empty;
            string articleContent = string.Empty;
            for (int i = 0; i < _currentWorkspace.Articles.Count; i++)
            {
                articleContent = CreateContentItem(_currentWorkspace.Articles[i], (i + 1).ToString());
                result += articleContent;
            }

            return result;
        }

        private string CreateContentItem(Article article, string index)
        {
            string path = Path.Combine(Template.Rootdir, Template.Articletpl);
            string text = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            text = text.Replace(ArticleIndex, index);
            text = text.Replace(ArticleText, article.ArticleText);
            text = text.Replace(Author, article.Author);
            text = text.Replace(PublicDate, string.Empty);
            text = text.Replace(Source, article.Source);
            text = text.Replace(SourceNumber, article.SourceNumber);
            text = text.Replace(Title, article.Title);
            text = text.Replace(AlsoIn, string.Empty);

            if (article.Images.Count > 0)
            {
                text = ExportImages(article, text);
            }

            return text;
        }

        private string ExportImages(Article article, string text)
        {
            string newPath, oldPath, image, imageDirectory;

            if (!Directory.Exists(ResultResourceDirectory))
            {
                Directory.CreateDirectory(ResultResourceDirectory);
            }

            foreach (string img in article.Images)
            {
                if (isUrl(img))
                    continue;
                image = ToBackslashString(img);
                oldPath = Path.Combine(Path.GetDirectoryName(article.Filepath), image);
                imageDirectory = CreateImageDirectory(image);

                newPath = Path.Combine(imageDirectory, Path.GetFileName(image));
                if (File.Exists(oldPath))
                {
                    File.Copy(oldPath, newPath);
                    text = text.Replace(img, newPath);
                }
            }
            return text;
        }

        private bool isUrl(string str)
        {
            return str.StartsWith(Http);
        }

        private string CreateImageDirectory(string image)
        {
            string imageDirectory = Path.GetDirectoryName(image);
            imageDirectory = Path.Combine(ResultResourceDirectory, imageDirectory);
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            return imageDirectory;
        }


        private string ToBackslashString(string img)
        {
            var image = img;
            if (image.StartsWith("/"))
            {
                image = image.Substring(1);
            }
            image = image.Replace("/", @"\");

            return image;
        }

        #endregion

        private string ExportTemplateImages(string text)
        {
            //TODO Implement method
            return text;
        }


        private Report CreateReport(string result, string resultPath)
        {
            //TODO implement clever logic when file already exist is it needed ?

            File.WriteAllText(resultPath, result, Encoding.GetEncoding(Importer.TextEncoding));
            Report report = new Report() { FilePath = resultPath };

            return report;
        }

        private string ResultDirectory
        {
            get 
            {
                return Path.GetDirectoryName(_resultPath);
            }
        }

        private string ResultName
        {
            get
            {
                return Path.GetFileName(_resultPath);
            }
        }

        private string ResultResourceDirectory
        {
            get
            {
                return Path.GetFileNameWithoutExtension(_resultPath);
            }
        }

        #endregion


    }
}
