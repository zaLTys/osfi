using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma3
    {
       public long ImonesKodas { get; set; }
       public string Pavadinimas { get; set; }
       public string Kodas { get; set; }
       public decimal IsViso { get; set; }
       public decimal? Augalininkyste { get; set; }
       public decimal? Gyvulininkyste { get; set; }

       public IDictionary<int, List<FormosKlaida>> Klaidos { get; set; }
    }
}