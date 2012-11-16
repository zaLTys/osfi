using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma8
    {
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public decimal MetuPradzioje { get; set; }
        public decimal MetuPabaigojeVnt { get; set; }
        public decimal MetuPabaigojeVerte { get; set; }
    }
}