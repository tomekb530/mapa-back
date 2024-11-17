using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace mapa_back.Models.RSPOApi
{
    public class SchoolFromMap
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("rspo_number")]
        public int RspoNumber { get; set; }

        [Column("business_data")]
        public string BusinessData { get; set; } = string.Empty;

        [Column("geography")]
        public Point Geography { get; set; }
    }
}
