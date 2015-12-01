using System.Collections.Generic;

namespace Core.Command
{
    public interface ICommandManager : ICommandExecutor
    {
        void Delete(Workspace workspace, IList<Article> articles);

        void DeleteAll(Workspace workspace);

        void DeleteImage(Article article);

        void DeleteAllImages(Workspace workspace);

    }
}
