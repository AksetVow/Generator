
using System.Collections.Generic;
namespace Core.Import
{
    public class ImportData
    {
        private IList<string> _files;

        public ImportData(IList<string> files)
        {
            _files = files;
        }

        public IList<string> FilePathes
        { 
            get { return _files; } 
        }

    }
}
