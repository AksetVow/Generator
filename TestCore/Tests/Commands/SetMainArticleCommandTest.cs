using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.Collections.Generic;
using Core.Command.Commands;

namespace TestCore.Tests.Commands
{
    [TestClass]
    public class SetMainArticleCommandTest
    {
        private SetMainArticleCommand CreateStubSetMainArticleCommand(out IList<Article> articles)
        {
            articles = new List<Article>();
            articles.Add(new Article() { IdMain = 2 });
            articles.Add(new Article() { IdMain = 3 });
            articles.Add(new Article() { IdMain = 4 });

            return new SetMainArticleCommand(7, articles);
        }


        [TestMethod]
        public void DoTest()
        {
            IList<Article> articles;

            var setMainArticleCommand = CreateStubSetMainArticleCommand(out articles);
            setMainArticleCommand.Do();

            Assert.AreEqual(7, articles[0].IdMain);
            Assert.AreEqual(7, articles[1].IdMain);
            Assert.AreEqual(7, articles[2].IdMain);
        }

        [TestMethod]
        public void UndoTest()
        {
            IList<Article> articles;

            var setMainArticleCommand = CreateStubSetMainArticleCommand(out articles);
            setMainArticleCommand.Do();

            Assert.AreEqual(7, articles[0].IdMain);
            Assert.AreEqual(7, articles[1].IdMain);
            Assert.AreEqual(7, articles[2].IdMain);

            setMainArticleCommand.Undo();

            Assert.AreEqual(2, articles[0].IdMain);
            Assert.AreEqual(3, articles[1].IdMain);
            Assert.AreEqual(4, articles[2].IdMain);


        }

        [TestMethod]
        public void RedoTest()
        {
            IList<Article> articles;

            var setMainArticleCommand = CreateStubSetMainArticleCommand(out articles);
            setMainArticleCommand.Do();
            setMainArticleCommand.Undo();
            setMainArticleCommand.Redo();

            Assert.AreEqual(7, articles[0].IdMain);
            Assert.AreEqual(7, articles[1].IdMain);
            Assert.AreEqual(7, articles[2].IdMain);

        }

    }
}
