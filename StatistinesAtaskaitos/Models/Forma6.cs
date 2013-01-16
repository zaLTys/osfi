using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma6
    {
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public decimal? VidutinisGyvuliuSk { get; set; }
        public decimal? ProdukcijosKiekis { get; set; }
        public decimal? IslaidosDarboApmokejimas { get; set; }
        public decimal? IslaidosPasarai { get; set; }
        public decimal? IslaidosNafta { get; set; }
        public decimal? IslaidosElektra { get; set; }
        public decimal? IslaidosKitos { get; set; }
        public decimal? IslaidosVisos { get; set; }
        public decimal? IslaidosPagrindinei { get; set; }
        public decimal? ProdukcijosSavikaina { get; set; }

        public IDictionary<int, List<FormosKlaida>> Klaidos { get; set; }
    }
}