using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System.Collections.Generic;
using Core.Command.Commands;

namespace TestCore.Tests.Commands
{
    [TestClass]
    public class AddCategoryCommandTest
    {
        private AddCategoryCommand CreateStubAddCategoryCommand(out IList<Article> articles)
        {
            articles = new List<Article>();
            articles.Add(new Article() { SubjectCategory = "Львів" });
            articles.Add(new Article() { SubjectCategory = "Банки" });
            articles.Add(new Article() { SubjectCategory = "Новини конкурентів" });

            return new AddCategoryCommand("Київ", articles);
        }


        [TestMethod]
        public void DoTest()
        {
            IList<Article> articles;

            var addCategoryCommand = CreateStubAddCategoryCommand(out articles);
            addCategoryCommand.Do();

            Assert.AreEqual("Київ", articles[0].SubjectCategory);
            Assert.AreEqual("Київ", articles[1].SubjectCategory);
            Assert.AreEqual("Київ", articles[2].SubjectCategory);
        }

        [TestMethod]
        public void UndoTest()
        {
            IList<Article> articles;

            var addCategoryCommand = CreateStubAddCategoryCommand(out articles);
            addCategoryCommand.Do();

            Assert.AreEqual("Київ", articles[0].SubjectCategory);
            Assert.AreEqual("Київ", articles[1].SubjectCategory);
            Assert.AreEqual("Київ", articles[2].SubjectCategory);

            addCategoryCommand.Undo();

            Assert.AreEqual("Львів", articles[0].SubjectCategory);
            Assert.AreEqual("Банки", articles[1].SubjectCategory);
            Assert.AreEqual("Новини конкурентів", articles[2].SubjectCategory);


        }

        [TestMethod]
        public void RedoTest()
        {
            IList<Article> articles;

            var addCategoryCommand = CreateStubAddCategoryCommand(out articles);
            addCategoryCommand.Do();
            addCategoryCommand.Undo();
            addCategoryCommand.Redo();

            Assert.AreEqual("Київ", articles[0].SubjectCategory);
            Assert.AreEqual("Київ", articles[1].SubjectCategory);
            Assert.AreEqual("Київ", articles[2].SubjectCategory);

        }
    }
}
