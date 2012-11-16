using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma7
    {
        public string Kodas { get; set; }
        public string Pavadinimas { get; set; }
        public decimal MetuPradziosLikutis { get; set; }
        public decimal PajamosPagaminta { get; set; }
        public decimal PajamosPirkta { get; set; }
        public decimal PajamosImportuota { get; set; }
        public decimal IslaidosVisos { get; set; }
        public decimal IslaidosParduota { get; set; }
        public decimal IslaidosPasarui { get; set; }
        public decimal? IslaidosSeklai { get; set; }
        public decimal IslaidosDuotaPerdirbti { get; set; }
        public decimal IslaidosProdukcijosNuostoliai { get; set; }
        public decimal IslaidosKitos { get; set; }
        public decimal MetupabaigosLikutis { get; set; }
    }
}
