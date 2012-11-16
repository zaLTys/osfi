using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class ProduktuPardavimas : IExcelioLentele<ProduktuPardavimoRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual ProduktuPardavimoRusis Rusis { get; set; }
        public virtual decimal? ParduotaNatura { get; set; }
        public virtual decimal? ParduotaEksportui { get; set; }
        public virtual decimal? ParduotaIskaitomuojuSvoriu { get; set; }
        public virtual decimal ProdukcijosSavikaina { get; set; }
        public virtual decimal PardavimuPajamos { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
