using Core;
using System.Collections.Generic;

namespace Generator.Utils
{
    static class ArticleHelper
    {
        public static bool ContainsImages(IList<Article> articles)
        {
            foreach (var article in articles)
            {
                if (article.Images.Count > 0)
                    return true;
            }
            return false;
        }

    }
}
