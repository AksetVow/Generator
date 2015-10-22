using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Workspace
    {
        private List<Article> _articles = new List<Article>();

        public void Add(Article article)
        {
            _articles.Add(article);
        }

        public void Add(List<Article> articles)
        {
            _articles.AddRange(articles);
        }

        public bool Remove(Article article)
        {
            return _articles.Remove(article);
        }

        public List<Article> RemoveAll()
        {
            var clonedList = _articles.Select(item => item).ToList();
            _articles.Clear();

            return clonedList;
        }

        public IEnumerable<Article> Articles
        {
            get
            {
                return _articles;
            }
        }

        

    }
}
