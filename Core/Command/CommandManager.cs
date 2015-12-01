using Core.Command.Commands;
using System;

namespace Core.Command
{
    public class CommandManager : ICommandManager
    {
        private ICommandExecutor _commandExecutor = new CommandExecutor();

        public void Delete(Workspace workspace, Article article)
        {
            var deleteCommand = new DeleteCommand(workspace, article);
            _commandExecutor.ExecuteCommand(deleteCommand);
        }

        public void DeleteAll(Workspace workspace)
        {
            var deleteAllCommand = new DeleteAllCommand(workspace);
            _commandExecutor.ExecuteCommand(deleteAllCommand);
        }

        public void DeleteImage(Article article)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllImages(Workspace workspace)
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            _commandExecutor.Undo();
        }

        public void Redo()
        {
            _commandExecutor.Redo();
        }

        public void Clean()
        {
            _commandExecutor.Clean();
        }

        public bool CanUndo()
        {
            return _commandExecutor.CanUndo();
        }

        public bool CanRedo()
        {
            return _commandExecutor.CanRedo();
        }
    }
}
