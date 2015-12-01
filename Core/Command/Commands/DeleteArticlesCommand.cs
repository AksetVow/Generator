using System;
using System.Collections.Generic;

namespace Core.Command.Commands
{
    public class DeleteArticlesCommand: ICommand
    {
        private Workspace _workspace;
        private IList<Article> _articles;
        private IList<Tuple<Article, int>> _articlesOnPosition;

        public DeleteArticlesCommand(Workspace workspace, IList<Article> articles)
        {
            if (articles == null || workspace == null)
                throw new ArgumentNullException("argument is incorret");

            _workspace = workspace;
            _articles = articles;
            _articlesOnPosition = new List<Tuple<Article, int>>();
        }

        public void Do()
        {
            int index;
            foreach (var article in _articles)
            {
                index = _workspace.Articles.IndexOf(article);
                _articlesOnPosition.Add(new Tuple<Article, int>(article, index));
                _workspace.Articles.Remove(article);
            }

        }

        public void Undo()
        {
            for (int j = _articlesOnPosition.Count - 1; j >= 0; j--)
            {
                _workspace.Articles.Insert(_articlesOnPosition[j].Item2, _articlesOnPosition[j].Item1);
            }
            
            _articlesOnPosition.Clear();
        }

        public void Redo()
        {
            Do();
        }
    }
}
