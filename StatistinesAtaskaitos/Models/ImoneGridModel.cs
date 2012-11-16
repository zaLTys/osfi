using System;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class ImoneGridModel
    {
        public int Id { get; set; }
        public long AsmensKodas { get; set; }
        public string Pavadinimas { get; set; }
        public ImonesStatusas Statusas { get; set; }
    }

    public enum ImonesStatusas
    {
        Patvirtinta,
        LaukiamaPatvirtinimo,
        Atmesta
    }
}