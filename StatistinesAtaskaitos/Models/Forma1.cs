using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma1
    {
        public long ImonesKodas { get; set; }
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public decimal LikutisPradziojeIlgalaikio { get; set; }
        public decimal Gauta { get; set; }
        public decimal IsJuNauju { get; set; }
        public decimal VertesPadidejimas { get; set; }
        public decimal NurasytaIlgalaikio { get; set; }
        public decimal LikviduotaIlgalaikio { get; set; }
        public decimal ParduotaIlgalaikio { get; set; }
        public decimal Nukainota { get; set; }
        public decimal LikutisPabaigojeIlgalaikio { get; set; }
        public decimal? LikutisPradziojeNusidevejimo { get; set; }
        public decimal? Priskaiciuota { get; set; }
        public decimal? Pasikeitimas { get; set; }
        public decimal? NurasytaNusidevejimo { get; set; }
        public decimal? LikviduotaNusidevejimo { get; set; }
        public decimal? LikutisPabaigojeNusidevejimo { get; set; }

        public IDictionary<int, List<FormosKlaida>> Klaidos { get; set; }
    }
}