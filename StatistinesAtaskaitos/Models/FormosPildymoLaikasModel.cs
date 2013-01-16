using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class FormosPildymoLaikasModel
    {
        public int Valandos { get; set; }
        public int Minutes { get; set; }

        public IDictionary<int, List<FormosKlaida>> Klaidos { get; set; }
    }
}