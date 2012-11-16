using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class IlgalaikisTurtas : IExcelioLentele<IlgalaikioTurtoRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual IlgalaikioTurtoRusis Rusis { get; set; }
        public virtual decimal LikutisPradziojeIlgalaikio { get; set; }
        public virtual decimal Gauta { get; set; }
        public virtual decimal IsJuNauju { get; set; }
        public virtual decimal VertesPadidejimas { get; set; }
        public virtual decimal NurasytaIlgalaikio { get; set; }
        public virtual decimal LikviduotaIlgalaikio { get; set; }
        public virtual decimal ParduotaIlgalaikio { get; set; }
        public virtual decimal Nukainota { get; set; }
        public virtual decimal LikutisPabaigojeIlgalaikio { get; set; }
        public virtual decimal? LikutisPradziojeNusidevejimo { get; set; }
        public virtual decimal? Priskaiciuota { get; set; }
        public virtual decimal? Pasikeitimas { get; set; }
        public virtual decimal? NurasytaNusidevejimo { get; set; }
        public virtual decimal? LikviduotaNusidevejimo { get; set; }
        public virtual decimal? LikutisPabaigojeNusidevejimo { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
