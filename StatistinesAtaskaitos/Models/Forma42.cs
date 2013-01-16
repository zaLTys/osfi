using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma42
    {
        public string Pavadinimas { get; set; }
        public string Kodas { get; set; }
        public decimal Suma { get; set; }

        public IDictionary<int, List<FormosKlaida>> Klaidos { get; set; }
    }
}