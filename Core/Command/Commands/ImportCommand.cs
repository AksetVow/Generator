using Core.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Command.Commands
{
    public class ImportCommand: ICommand
    {
        private Importer _importer;
        private Workspace _workspace;
        private IList<Article> _articles;
        private string[] _filenames;

        public ImportCommand(Importer importer, Workspace workspace, string[] filenames)
        {
            _importer = importer;
            _filenames = filenames;
            _workspace = workspace;
        }


        public void Do()
        {
            _articles = _importer.Import(_filenames);
            _workspace.Add(_articles);
        }

        public void Undo()
        {
            foreach (var article in _articles)
            {
                _workspace.Articles.Remove(article);
            }
        }

        public void Redo()
        {
            _workspace.Add(_articles);
        }
    }
}
