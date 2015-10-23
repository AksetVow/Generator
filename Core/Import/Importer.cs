using System;
using System.Collections.Generic;

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


        public Article ImportFile(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            throw new NotImplementedException();
        }

        public IList<Article> ImportArchive(string filepath)
        {
            if (ImportConfiguration == null)
                throw new NullReferenceException("No import configuration");

            throw new NotImplementedException();
        }



    }
}
