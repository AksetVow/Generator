using System.Collections.Generic;
using System.Linq;

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

        public int Mark { get; set; }
        public int Id { get; set; }
        public int IdMain { get; set; }


        public Article Copy()
        {
            Article copy = new Article();

            copy.ArticleText = ArticleText;
            copy.Author = Author;
            copy.Category = Category;
            copy.CategoryEmpty = CategoryEmpty;
            copy.Filepath = Filepath;
            copy.Id = Id;
            copy.IdMain = IdMain;
            copy.KeyWords = KeyWords;
            copy.Mark = Mark;
            copy.PublicDate = PublicDate;
            copy.Region = Region;
            copy.Source = Source;
            copy.SourceNumber = SourceNumber;
            copy.SubjectCategory = SubjectCategory;
            copy.Title = Title;

            copy.Images = (Images as List<string>).ToList();

            return copy;
        }

        public void CopyFrom(Article article)
        {
            ArticleText = article.ArticleText;
            Author = article.Author;
            Category = article.Category;
            CategoryEmpty = article.CategoryEmpty;
            Filepath = article.Filepath;
            Id = article.Id;
            IdMain = article.IdMain;
            KeyWords = article.KeyWords;
            Mark = article.Mark;
            PublicDate = article.PublicDate;
            Region = article.Region;
            Source = article.Source;
            SourceNumber = article.SourceNumber;
            SubjectCategory = article.SubjectCategory;
            Title = article.Title;

            Images = (article.Images as List<string>).ToList();
        }
    }
}
