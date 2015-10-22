
namespace Core.Import
{
    public class ImportConfiguration
    {
        public string Name { get; set; }
        public bool IsArchive { get; set; }
        public string FileMask { get; set; }
        public string Reseparator { get; set; }
        public string Regetarticletext { get; set; }
        public string Regetauthor { get; set; }
        public string Regetpublicdate { get; set; }
        public string Regetsource { get; set; }
        public string Regetsourcenumber { get; set; }
        public string Regettitle { get; set; }
        public string Regetcategory { get; set; }
        public string Regetregion { get; set; }
        public string Categoryempty { get; set; }
        public string Regetkeywords { get; set; }
    }
}
