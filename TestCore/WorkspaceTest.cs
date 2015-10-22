using Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TestCore
{
    [TestClass]
    public class WorkspaceTest
    {
        private Workspace _workspace;

        [TestInitialize]
        public void Initialize()
        {
            _workspace = new Workspace();
        }


        [TestMethod]
        public void TestAdd()
        {
            Assert.AreEqual(_workspace.Articles.Count, 0);
            _workspace.Add(new Article());

            Assert.AreEqual(_workspace.Articles.Count, 1);
        }

        [TestMethod]
        public void TestAddList()
        {
            Assert.AreEqual(_workspace.Articles.Count, 0);
            var list = new List<Article>();
            list.Add(new Article());
            list.Add(new Article());
            list.Add(new Article());
            list.Add(new Article());
            _workspace.Add(list);

            Assert.AreEqual(_workspace.Articles.Count, 4);
        }


        [TestMethod]
        public void TestRemove()
        {
            Assert.AreEqual(_workspace.Articles.Count, 0);
            var article = new Article();
            _workspace.Add(article);
            Assert.AreEqual(_workspace.Articles.Count, 1);
            Assert.IsTrue(_workspace.Remove(article));
            Assert.AreEqual(_workspace.Articles.Count, 0);

        }


        [TestMethod]
        public void TestRemoveAll()
        {
            Assert.AreEqual(_workspace.Articles.Count, 0);
            _workspace.Add(new Article());
            _workspace.Add(new Article());
            _workspace.Add(new Article());
            _workspace.Add(new Article());
            Assert.AreEqual(_workspace.Articles.Count, 4);
            var cloned = _workspace.RemoveAll();
            Assert.AreEqual(_workspace.Articles.Count, 0);
            Assert.AreEqual(cloned.Count, 4);
        }


        [TestCleanup]
        public void CleanUp()
        {
            _workspace.RemoveAll();
            _workspace = null;
        }
    }
}
