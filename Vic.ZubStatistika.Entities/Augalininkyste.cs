using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Augalininkyste : IExcelioLentele<AugalininkystesRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual AugalininkystesRusis Rusis { get; set; }
        public virtual decimal? Plotas { get; set; }
        public virtual decimal? ProdukcijosKiekis { get; set; }
        public virtual decimal? Derlingumas { get; set; }
        public virtual decimal IslaidosDarboApmokejimas { get; set; }
        public virtual decimal IslaidosSeklos { get; set; }
        public virtual decimal IslaidosTrasos { get; set; }
        public virtual decimal IslaidosNafta { get; set; }
        public virtual decimal IslaidosElektra { get; set; }
        public virtual decimal IslaidosKitos { get; set; }
        public virtual decimal IslaidosVisos { get; set; }
        public virtual decimal? IslaidosPagrindinei { get; set; }
        public virtual decimal? ProdukcijosSavikaina { get; set; }
        public virtual Upload Upload { get; set; }

    }
}
