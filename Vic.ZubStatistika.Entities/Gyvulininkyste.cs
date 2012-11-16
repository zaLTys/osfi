using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Gyvulininkyste : IExcelioLentele<GyvulininkystesRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual GyvulininkystesRusis Rusis { get; set; }
        public virtual decimal? VidutinisGyvuliuSk { get; set; }
        public virtual decimal? ProdukcijosKiekis { get; set; }
        public virtual decimal? IslaidosDarboApmokejimas { get; set; }
        public virtual decimal? IslaidosPasarai { get; set; }
        public virtual decimal? IslaidosNafta { get; set; }
        public virtual decimal? IslaidosElektra { get; set; }
        public virtual decimal? IslaidosKitos { get; set; }
        public virtual decimal? IslaidosVisos { get; set; }
        public virtual decimal? IslaidosPagrindinei { get; set; }
        public virtual decimal? ProdukcijosSavikaina { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
