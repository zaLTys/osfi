using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Sanaudos : IExcelioLentele<SanauduRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual SanauduRusis Rusis { get; set; }
        public virtual decimal IsViso { get; set; }
        public virtual decimal? Augalininkyste { get; set; }
        public virtual decimal? Gyvulininkyste { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
