using mapa_back.Models.RSPOApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapa_back.Models.DTO
{
    public class ManagingEntityDTO
    {
        public ManagingEntityDTO(int? id, string? nazwa, string? typ, string? regon)
        {
            Id = id;
            Nazwa = nazwa;
            Typ = typ;
            Regon = regon;
        }

        public int? Id { get; set; }
        public string? Nazwa { get; set; }
        public string? Typ { get; set; }
        public string? Regon { get; set; }
    }
}
