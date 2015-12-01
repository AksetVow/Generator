using System;
using System.Collections.Generic;

namespace Core.Command.Commands
{
    public class DeleteAllCommand : ICommand
    {
        private Workspace _workspace;
        private IList<Article> _deleteArticles;

        public DeleteAllCommand(Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace couldn't be null");

            _workspace = workspace;
        }

        public void Do()
        {
            _deleteArticles = _workspace.RemoveAll();
        }

        public void Undo()
        {
            _workspace.Add(_deleteArticles);
        }

        public void Redo()
        {
            Do();
        }
    }
}
