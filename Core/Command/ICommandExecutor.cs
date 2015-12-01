
namespace Core.Command
{
    public interface ICommandExecutor
    {
        void ExecuteCommand(ICommand command);

        void Undo();

        void Redo();

        void Clean();

        bool CanUndo();

        bool CanRedo();

    }
}
