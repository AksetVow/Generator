using System;

namespace Core.Command.Commands
{
    public class DeleteCommand: ICommand
    {
        private Workspace _workspace;
        private Article _article;
        private int _index = -1;

        public DeleteCommand(Workspace workspace, Article article)
        {
            if (article == null || workspace == null)
                throw new ArgumentNullException("argument is incorret");

            _workspace = workspace;
            _article = article;
        }

        public void Do()
        {
            _index = _workspace.Articles.IndexOf(_article);
            _workspace.Remove(_article);
        }

        public void Undo()
        {
            _workspace.Articles.Insert(_index, _article);
        }

        public void Redo()
        {
            Do();
        }
    }
}
