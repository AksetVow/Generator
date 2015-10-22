

namespace Core.Command
{
    public interface ICommand
    {
        void Do();

        void Undo();

        void Redo();
    }
}
