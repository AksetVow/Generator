using Core.Import;
using Core.Utils;
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
        private int _exportImageCounter = 0;

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

        private const string IndentString = @"%INDENTSTRING%";
        private const string CountName = @"%CNTNAME%";
        private const string CountDifferent = @"%CNTCOUNT%";
        private const string SymbolCount = @"%SYMBOLCOUNT%";
        private const string CounterTableEmptyString = @"<пусто>";

        private const string Http = "http";
        private const string TemplateImageName = "_tplimg_";


        public Report Export(Workspace workspace, string resultPath, UserRequestData userRequestData, ExportCounterSettings counterSettings = null)
        {
            _exportImageCounter = 0;
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
            StringBuilder sb = new StringBuilder();
            sb.Append(CreateCountTableHeader());

            var tableContentItems = CounterExportTable.GetCountArticleTable(_currentWorkspace.Articles, _counterSettings);
            var tableGroupTemplate = CreateTableContent();
            var tableContentTemplate = CreateTableGroup();
            string row;

            int whiteSpaces = 0;
            string indent = string.Empty;
            foreach (var item in tableContentItems)
            {
                indent = Template.Countindentstr.Multiply(whiteSpaces);
                ++whiteSpaces;
                if (item.Item4)
                {
                    row = tableGroupTemplate;
                    whiteSpaces = 0;
                }
                else
                {
                    row = tableContentTemplate;
                }
                if (string.IsNullOrEmpty(item.Item1))
                {
                    row = row.Replace(CountName, CounterTableEmptyString);
                }
                else
                {
                    row = row.Replace(CountName, item.Item1);
                }
                row = row.Replace(CountDifferent, item.Item2.ToString());
                row = row.Replace(SymbolCount, item.Item3.ToString());
                row = row.Replace(IndentString, indent);
                sb.Append(row);
            }

            sb.Append(CreateCountTableFooter());

            return sb.ToString();
        }

        private string CreateTableContent()
        {
            string path = Path.Combine(Template.Rootdir, Template.Counttpl);
            string counterContentTpl = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));
            return counterContentTpl;
        }

        private string CreateTableGroup()
        {
            string path = Path.Combine(Template.Rootdir, Template.Countgrouptpl);
            string counterGroupTpl = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));
            return counterGroupTpl;
        }

        private string CreateCountTableHeader()
        {
            string path = Path.Combine(Template.Rootdir, Template.Countheadertpl);
            string counterTableHeader = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));
            return counterTableHeader;
        }

        private string CreateCountTableFooter()
        {
            string path = Path.Combine(Template.Rootdir, Template.Countfootertpl);
            string counterTableFooter = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));
            return counterTableFooter;
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

            if (_counterSettings == null || _counterSettings.IsEmpty)
            {
                footer = footer.Replace(CountList, string.Empty);
            }
            else
            {
                var countTable = CreateCountTable();
                footer = footer.Replace(CountList, countTable);
            }

            footer = footer.Replace(Rating, string.Empty);

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

        #region ExportImagesMethods
        private string ExportTemplateImages(string text)
        {
            if (Template.Images == null)
            {
                return text;
            }

            for (int i = 0; i < Template.Images.Count; i++)
            {
                text = ExportTemplateImage(text, Template.Images[i], i);
            }

            return text;
        }

        private string ExportTemplateImage(string text, string image, int index)
        {
            if (isUrl(image))
            {
                return text;
            }

            if (!Directory.Exists(ResultResourceDirectory))
            {
                Directory.CreateDirectory(ResultResourceDirectory);
            }

            string oldPath = GetOldPathForTemplateImage(image);
            string newPath = GetNewPathForTemplateImage(image);
            string result = text.Replace(image, newPath);

            if (File.Exists(oldPath))
            {
                File.Copy(oldPath, newPath);
            }

            return result;
        }

        private string GetOldPathForTemplateImage(string image)
        {
            return Path.Combine(Template.Rootdir, image);
        }

        private string GetNewPathForTemplateImage(string image)
        {
            return Path.Combine(ResultResourceDirectory, TemplateImageName + _exportImageCounter++ + Path.GetExtension(image));
        }
        #endregion

        private Report CreateReport(string result, string resultPath)
        {
            //TODO implement clever logic when file already exist is it needed ?

            File.WriteAllText(resultPath, result, Encoding.GetEncoding(Importer.TextEncoding));
            Report report = new Report() { FilePath = resultPath };

            return report;
        }

        #region PivateProperties

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
                return Path.Combine(ResultDirectory, Path.GetFileNameWithoutExtension(_resultPath));
            }
        }

        #endregion

        #endregion


    }
}
