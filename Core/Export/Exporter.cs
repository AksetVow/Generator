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


        public Report Export(Workspace workspace, string resultPath, ExportCounterSettings counterSettings = null)
        {
            _currentWorkspace = workspace;
            _counterSettings = counterSettings;

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
        public string CreateToc()
        {
            throw new NotImplementedException();
        }

        public string CreateTocHeader()
        {
            throw new NotImplementedException();
        }

        public string CreateTocFooter()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public string CreateFooter()
        {
            throw new NotImplementedException();
        }

        public string CreateContent()
        {
            throw new NotImplementedException();
        }
        #endregion


        public Report CreateReport(string result, string resultPath)
        {
            //TODO implement clever logic when file already exist
            File.Create(resultPath);

            File.WriteAllText(resultPath, result, Encoding.GetEncoding(Importer.TextEncoding));
            Report report = new Report() { FilePath = resultPath };

            return report;
        }

        #endregion


    }
}
