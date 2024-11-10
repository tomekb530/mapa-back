using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mapa_back.Models
{
    public class School
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rspo_number")]
        public int RspoNumber { get; set; }

        [Column("business_data")]
        public string BusinessData { get; set; }

        [Column("geography")]
        public Point Geography { get; set; } 
    }
}
