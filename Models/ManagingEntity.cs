using mapa_back.Models.RSPOApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapa_back.Models
{
    public class ManagingEntity
    {
        public ManagingEntity(int? id, string? nazwa, RSPOTypeSchema? typ, string? regon)
        {
            Id = id;
            Nazwa = nazwa;
            Typ = typ;
            Regon = regon;
        }

        public int? Id { get; set; }
        public string? Nazwa { get; set; }
        public RSPOTypeSchema? Typ { get; set; }
        public string? Regon { get; set; }
    }
}
