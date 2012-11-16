using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma41
    {
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public decimal? ParduotaNatura { get; set; }
        public decimal? ParduotaEksportui { get; set; }
        public decimal? ParduotaIskaitomuojuSvoriu { get; set; }
        public decimal ProdukcijosSavikaina { get; set; }
        public decimal PardavimuPajamos { get; set; }
    }
}