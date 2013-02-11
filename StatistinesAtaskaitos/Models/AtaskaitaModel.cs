using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class AtaskaitaModel
    {
        public IDictionary<FormuTipai, int> KlaiduSkaicius { get; set; }
        public AtaskaitosParametrai Parametrai { get; set; }
        public object Rezultatai { get; set; }
    }
}