using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class DotacijosSubsidijos : IExcelioLentele<DotacijuSubsidijuRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual DotacijuSubsidijuRusis Rusis { get; set; }
        public virtual decimal Suma { get; set; }
        public virtual Upload Upload { get; set; }
    }


}
