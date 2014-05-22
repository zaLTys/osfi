using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma12
    {
        public long ImonesKodas { get; set; }
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public decimal FinansiniaiMetai { get; set; }
        public decimal PraejeFinansiniaiMetai { get; set; }


        public IDictionary<int, List<FormosKlaida>> Klaidos { get; set; }
    }
}