
namespace Core.Command
{
    public interface ICommandManager
    {
        void Delete(Workspace workspace, Article article);

        void DeleteAll(Workspace workspace);

        void DeleteImage(Article article);

        void DeleteAllImages(Workspace workspace);

        void Undo();

        void Redo();

        void Clean();

        bool CanUndo();

        bool CanRedo();

    }
}
