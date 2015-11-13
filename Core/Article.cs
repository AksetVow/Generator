
using System.Collections.Generic;
namespace Core
{
    public class Article
    {
        public string ArticleText { get; set; }
        public string Author { get; set; }
        public string PublicDate { get; set; }
        public string Source { get; set; }
        public string SourceNumber { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Region { get; set; }
        public string CategoryEmpty { get; set; }
        public string KeyWords { get; set; }

        public string Filepath { get; set; }
        public string SubjectCategory { get; set; }

        public IList<string> Images { get; set; }

        public string Mark { get; set; }
        public int Id { get; set; }
        public int IdMain { get; set; }

    }
}
