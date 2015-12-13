using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Command.Commands
{
    public class AddCategoryCommand:ICommand
    {
        private string _category;
        private List<Tuple<Article, string>> _articleCategories = new List<Tuple<Article, string>>();
        private IList<Article> _articles;


        public AddCategoryCommand(string category, IList<Article> articles)
        {
            _articles = articles;
            _category = category;

            SaveArticles();
        }


        public void Do()
        {
            foreach (var article in _articles)
            {
                article.SubjectCategory = _category;
            }
        }

        public void Undo()
        {
            foreach (var item in _articleCategories)
            {
                item.Item1.SubjectCategory = item.Item2;
            }
        }

        public void Redo()
        {
            Do();
        }

        private void SaveArticles()
        {
            foreach (var article in _articles)
            {
                _articleCategories.Add(new Tuple<Article, string>(article, article.SubjectCategory));
            }
        }
    }
}
