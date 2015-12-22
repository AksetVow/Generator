using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.Collections.Generic;
using System.Linq;

namespace TestCore.Tests
{
    [TestClass]
    public class ArticleTest
    {
        [TestMethod]
        public void TestCopy()
        {
            Article article = MockArticle();
            var articleCopy = article.Copy();
            CheckEqual(article, articleCopy);
        }

        [TestMethod]
        public void TestCopyWithNull()
        {
            Article article = MockArticle();
            article.Images = null;
            var articleCopy = article.Copy();
            CheckEqual(article, articleCopy);
        }

        [TestMethod]
        public void TestCopyWithEmptyImagesList()
        {
            Article article = MockArticle();
            article.Images = new List<string>();
            var articleCopy = article.Copy();
            CheckEqual(article, articleCopy);
        }

        [TestMethod]
        public void CopyFrom()
        {
            Article article = new Article();
            Article sourceArticle = MockArticle();

            article.CopyFrom(sourceArticle);

            CheckEqual(article, sourceArticle);
        }


        private void CheckEqual(Article article, Article articleCopy)
        {
            Assert.AreEqual(article.ArticleText, articleCopy.ArticleText);
            Assert.AreEqual(article.ArticleOriginText, articleCopy.ArticleOriginText);
            Assert.AreEqual(article.Author, articleCopy.Author);
            Assert.AreEqual(article.Category, articleCopy.Category);
            Assert.AreEqual(article.CategoryEmpty, articleCopy.CategoryEmpty);
            Assert.AreEqual(article.Filepath, articleCopy.Filepath);
            Assert.AreEqual(article.Id, articleCopy.Id);
            Assert.AreEqual(article.IdMain, articleCopy.IdMain);
            Assert.AreEqual(article.KeyWords, articleCopy.KeyWords);

            Assert.AreEqual(article.Mark, articleCopy.Mark);
            Assert.AreEqual(article.PublicDate, articleCopy.PublicDate);
            Assert.AreEqual(article.Region, articleCopy.Region);
            Assert.AreEqual(article.Source, articleCopy.Source);
            Assert.AreEqual(article.SourceNumber, articleCopy.SourceNumber);
            Assert.AreEqual(article.SubjectCategory, articleCopy.SubjectCategory);
            Assert.AreEqual(article.Title, articleCopy.Title);

            CollectionAssert.AreEqual(article.Images as List<string>, articleCopy.Images as List<string>);
        }

        private Article MockArticle()
        {
            Article article = new Article()
            {
                ArticleText = "wefwefw",
                ArticleOriginText = "wefwefw123gwerg",
                Author = "fwefwef",
                Category = "fwefwefcvc",
                CategoryEmpty = "wefwef",
                Filepath = "wefwthmywhy",
                Id = 123,
                IdMain = 4234,
                Images = new List<string>(),
                KeyWords = "fwefw",
                Mark = 12,
                PublicDate = "111.04.2014",
                Region = "Kyiv",
                Source = "fwefw",
                SourceNumber = "ewfwfe",
                SubjectCategory = "wefwefw",
                Title = "Title"
            };

            article.Images.Add("wefwf");
            article.Images.Add("gfgerg");

            return article;
        }
    }
}
