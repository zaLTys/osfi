using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class ZemesPlotai : IExcelioLentele<ZemesPlotuRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual ZemesPlotuRusis Rusis { get; set; }
        public virtual decimal NuomaIsValstybes { get; set; }
        public virtual decimal NuomaIsFiziniu { get; set; }
        public virtual decimal NuosavaZeme { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
