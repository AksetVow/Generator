using Core.Import;
using System.Collections.Generic;

namespace Core.Command
{
    public interface ICommandManager : ICommandExecutor
    {
        void Delete(Workspace workspace, IList<Article> articles);

        void DeleteAll(Workspace workspace);

        void DeleteImages(IList<Article> articles);

        void Import(Importer importer, Workspace workspace, string[] filenames);

        void SetMainId(IList<Article> articles, int id);

    }
}
