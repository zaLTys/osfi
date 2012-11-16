using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Darbuotojai : IExcelioLentele<DarbuotojuRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual DarbuotojuRusis Rusis { get; set; }
        public virtual decimal Reiksme { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
