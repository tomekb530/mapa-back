using mapa_back.Models.DTO;

namespace mapa_back.Models
{
    public class ChangedSchool
    {
        public SchoolDTO SchoolsBeforeChanges { get; set; }
        public SchoolDTO SchoolsAfterChanges { get; set; }
    }
}
