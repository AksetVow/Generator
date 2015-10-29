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

        private string ArticleIndex = @"%ARTICLEINDEX%";
        private string ArticleText = @"%ARTICLETEXT%";
        private string Author = @"%AUTHOR%";
        private string PublicDate =  @"%PUBLICDATE%";
        private string Source = @"%SOURCE%";
        private string SourceNumber = @"%SOURCENUMBER%";
        private string Title = @"%TITLE%";


        public Report Export(Workspace workspace, string resultPath, ExportCounterSettings counterSettings = null)
        {
            _currentWorkspace = workspace;
            _counterSettings = counterSettings;

            string result = string.Empty;
            
            result += CreateHeader();

            if (Template.IncludeToc && false)
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
        public string CreateToc()
        {
            throw new NotImplementedException();
        }

        public string CreateTocHeader()
        {
            string path = Path.Combine(Template.Rootdir, Template.Tocheadertpl);
            string tocHeader = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            return tocHeader;
        }

        public string CreateTocFooter()
        {
            string path = Path.Combine(Template.Rootdir, Template.Tocfootertpl);
            string tocFooter = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            return tocFooter;
        }

        #endregion 

        #region Counter

        public string CreateCountTable()
        {
            throw new NotImplementedException();
        }

        public string CreateCountTableHeader()
        {
            throw new NotImplementedException();
        }

        public string CreateCountTableFooter()
        {
            throw new NotImplementedException();
        }
        

        #endregion

        #region Content
        public string CreateHeader()
        {
            string path = Path.Combine(Template.Rootdir, Template.Headertpl);
            string header = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            return header;
        }

        public string CreateFooter()
        {
            string path = Path.Combine(Template.Rootdir, Template.Footertpl);
            string footer = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            return footer;
        }

        public string CreateContent()
        {
            string result = string.Empty;
            string articleContent = string.Empty;
            for (int i = 0; i < _currentWorkspace.Articles.Count; i++)
            {
                articleContent = CreateContent(_currentWorkspace.Articles[i], (i + 1).ToString());
                result += articleContent;
            }

            return result;
        }

        public string CreateContent(Article article, string index)
        {
            string path = Path.Combine(Template.Rootdir, Template.Articletpl);
            string text = File.ReadAllText(path, Encoding.GetEncoding(Importer.TextEncoding));

            text = text.Replace(ArticleIndex, index);
            text = text.Replace(ArticleText, article.ArticleText);
            text = text.Replace(Author, article.Author);
            text = text.Replace(PublicDate, article.PublicDate);
            text = text.Replace(Source, article.SourceNumber);
            text = text.Replace(Title, article.Title);

            return text;
        }

        #endregion


        public Report CreateReport(string result, string resultPath)
        {
            //TODO implement clever logic when file already exist is it needed ?

            File.WriteAllText(resultPath, result, Encoding.GetEncoding(Importer.TextEncoding));
            Report report = new Report() { FilePath = resultPath };

            return report;
        }

        #endregion


    }
}
