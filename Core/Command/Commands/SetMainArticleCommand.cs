using System;
using System.Collections.Generic;

namespace Core.Command.Commands
{
    public class SetMainArticleCommand:ICommand
    {
        private int _id;
        private List<Tuple<Article, int>> _articleIdMain = new List<Tuple<Article, int>>();
        private IList<Article> _articles;


        public SetMainArticleCommand(int id, IList<Article> articles)
        {
            _articles = articles;
            _id = id;

            SaveArticles();
        }


        public void Do()
        {
            foreach (var article in _articles)
            {
                article.IdMain = _id;
            }
        }

        public void Undo()
        {
            foreach (var item in _articleIdMain)
            {
                item.Item1.IdMain = item.Item2;
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
                _articleIdMain.Add(new Tuple<Article, int>(article, article.IdMain));
            }
        }
    }
}
