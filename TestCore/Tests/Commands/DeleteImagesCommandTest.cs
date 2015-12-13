using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Command.Commands;
using Core;
using System.IO;
using System.Collections.Generic;

namespace TestCore.Tests
{
    [TestClass]
    public class DeleteImagesCommandTest
    {

        private DeleteImagesCommand CreateStubDeleteImagesCommand(out Article article)
        {
            article = new Article()
            {
                ArticleText = File.ReadAllText("articletext.txt"),
                Images = new List<string>()
            };
            article.Images.Add("10960593_files/1.jpg");
            article.Images.Add("10960593_files/2.jpg");

            var articles = new List<Article>();
            articles.Add(article);

            var deleteImagesCommand = new DeleteImagesCommand(articles);

            return deleteImagesCommand;
        }


        [TestMethod]
        public void DoTest()
        {
            Article article;
            var deleteImagesCommand = CreateStubDeleteImagesCommand(out article);

            deleteImagesCommand.Do();
            string str = File.ReadAllText("articlewithoutimages.txt");

            Assert.AreEqual(article.Images.Count, 0);
            Assert.AreEqual(article.ArticleText, str);
        }

        [TestMethod]
        public void UndoTest()
        {
            Article article;
            var deleteImagesCommand = CreateStubDeleteImagesCommand(out article);

            deleteImagesCommand.Do();
            deleteImagesCommand.Undo();
            string str = File.ReadAllText("articletext.txt");

            Assert.AreEqual(article.Images.Count, 2);
            Assert.AreEqual(article.ArticleText, str);
        }


        [TestMethod]
        public void RedoTest()
        {
            Article article;
            var deleteImagesCommand = CreateStubDeleteImagesCommand(out article);

            deleteImagesCommand.Do();
            deleteImagesCommand.Undo();
            deleteImagesCommand.Redo();
            string str = File.ReadAllText("articlewithoutimages.txt");

            Assert.AreEqual(article.Images.Count, 0);
            Assert.AreEqual(article.ArticleText, str);
        }

    }
}
