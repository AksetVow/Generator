using Core.Command.Commands;
using System;
using System.Collections.Generic;

namespace Core.Command
{
    public class CommandManager : ICommandManager
    {
        private ICommandExecutor _commandExecutor = new CommandExecutor();

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

        public void ExecuteCommand(ICommand command)
        {
            _commandExecutor.ExecuteCommand(command);
        }

        public void Delete(Workspace workspace, IList<Article> articles)
        {
            var deleteCommand = new DeleteArticlesCommand(workspace, articles);
            _commandExecutor.ExecuteCommand(deleteCommand);
        }

        public void DeleteAll(Workspace workspace)
        {
            var deleteAllCommand = new DeleteAllCommand(workspace);
            _commandExecutor.ExecuteCommand(deleteAllCommand);
        }

        public void DeleteImages(IList<Article> articles)
        {
            var deleteImagesCommand = new DeleteImagesCommand(articles);
            _commandExecutor.ExecuteCommand(deleteImagesCommand);
        }

        public void Import(Import.Importer importer, Workspace workspace, string[] filenames)
        {
            var importCommand = new ImportCommand(importer, workspace, filenames);
            _commandExecutor.ExecuteCommand(importCommand);
        }

        public void SetMainId(IList<Article> articles, int id)
        {
            var setMainArticleCommand = new SetMainArticleCommand(id, articles);
            _commandExecutor.ExecuteCommand(setMainArticleCommand);
        }

        public void AddCategory(IList<Article> articles, string category)
        {
            var addCategoryCommand = new AddCategoryCommand(category, articles);
            _commandExecutor.ExecuteCommand(addCategoryCommand);
        }
    }
}
