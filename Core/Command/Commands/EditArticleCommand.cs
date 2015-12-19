namespace Core.Command.Commands
{
    public class EditArticleCommand:ICommand
    {
        private Article _originCopy;
        private Article _origin;
        private Article _updated;

        public EditArticleCommand(Article origin, Article updated)
        {
            _origin = origin;
            _originCopy = _origin.Copy();
            _updated = updated;
        }

        public void Do()
        {
            _origin.CopyFrom(_updated);
        }

        public void Undo()
        {
            _origin.CopyFrom(_originCopy);
        }

        public void Redo()
        {
            Do();
        }
    }
}
