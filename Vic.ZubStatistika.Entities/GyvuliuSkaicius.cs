using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class GyvuliuSkaicius : IExcelioLentele<GyvuliuSkaiciausRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual GyvuliuSkaiciausRusis Rusis { get; set; }
        public virtual decimal MetuPradzioje { get; set; }
        public virtual decimal MetuPabaigojeVnt { get; set; }
        public virtual decimal MetuPabaigojeVerte { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
