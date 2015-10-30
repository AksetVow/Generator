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

        private string ArticleIndex = @"%ARTICLEINDEX%";
        private string ArticleText = @"%ARTICLETEXT%";
        private string Author = @"%AUTHOR%";
        private string PublicDate =  @"%PUBLICDATE%";
        private string Source = @"%SOURCE%";
        private string SourceNumber = @"%SOURCENUMBER%";
        private string Title = @"%TITLE%";


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

            if (_counterSettings != null)
            {
                result += CreateCountTableHeader();
                result += CreateCountTable();
                result += CreateCountTableFooter();
            }

            result += CreateFooter();

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
            text = text.Replace(PublicDate, article.PublicDate);
            text = text.Replace(Source, article.Source);
            text = text.Replace(SourceNumber, article.SourceNumber);
            text = text.Replace(Title, article.Title);

            return text;
        }

        #endregion


        private Report CreateReport(string result, string resultPath)
        {
            //TODO implement clever logic when file already exist is it needed ?

            File.WriteAllText(resultPath, result, Encoding.GetEncoding(Importer.TextEncoding));
            Report report = new Report() { FilePath = resultPath };

            return report;
        }

        #endregion


    }
}
