using mapa_back.Models.DTO;

namespace mapa_back.Models
{
    public class ChangedSchools
    {
        public List<SchoolDTO> SchoolsBeforeChanges { get; set; }
        public List<SchoolDTO> SchoolsAfterChanges { get; set; }

        public ChangedSchools()
        {
            SchoolsAfterChanges = new List<SchoolDTO>();
            SchoolsBeforeChanges = new List<SchoolDTO>();
        }
    }
}
