using mapa_back.Models.DTO;

namespace mapa_back.Models
{
    public class ChangedSchool
    {
        public List<SchoolDTO> SchoolsBeforeChanges { get; set; }
        public List<SchoolDTO> SchoolsAfterChanges { get; set; }
    }
}
