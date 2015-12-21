
namespace Core.Export
{
    public class ExportCounterSettings
    {
        public bool Author { get; set; }
        public bool Category { get; set; }
        public bool SubjectCategory { get; set; }
        public bool PublicationDate { get; set; }
        public bool Source { get; set; }
        public bool Title { get; set; }


        public bool IsEmpty
        {
            get
            {
                return !(Author || Category || SubjectCategory || PublicationDate || Source || Title);
            }
        
        }
    }
}
