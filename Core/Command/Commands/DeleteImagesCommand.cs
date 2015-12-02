using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core.Import;

namespace Core.Command.Commands
{
    public class DeleteImagesCommand:ICommand
    {
        private IList<Article> _articles;
        private IList<Article> _articlesWithImages;


        public DeleteImagesCommand(IList<Article> articles)
        {
            _articles = articles;
            _articlesWithImages = _articles.Select(item => item.Copy()).ToList();
        }

        public void Do()
        {
            foreach (var article in _articles) {
                DeleteImages(article);
            }

        }

        public void Undo()
        {
            for (int i = 0; i < _articles.Count; i++ )
            {
                _articles[i].ArticleText = _articlesWithImages[i].ArticleText;
                _articles[i].Images = _articlesWithImages[i].Images;
            }
        }

        public void Redo()
        {
            Do();
        }

        private void DeleteImages(Article article)
        {
            article.Images.Clear();
            article.ArticleText = Regex.Replace(article.ArticleText, Importer.ImageRegex, "");
        }

    }
}
