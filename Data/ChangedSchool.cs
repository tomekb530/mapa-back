using mapa_back.Models.DTO;

namespace mapa_back.Models
{
    public class ChangedSchool
    {
        public SchoolDTO SchoolBeforeChanges { get; set; }
        public SchoolDTO SchoolsAfterChanges { get; set; }

        public ChangedSchool(SchoolDTO schoolBeforeChanges, SchoolDTO schoolAfterChanges)
        {
            this.SchoolBeforeChanges = schoolBeforeChanges;
            this.SchoolsAfterChanges = schoolAfterChanges;
		}
    }
}
