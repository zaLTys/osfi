using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class ProdukcijosKaita : IExcelioLentele<ProdukcijosKaitosRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual ProdukcijosKaitosRusis Rusis { get; set; }
        public virtual decimal MetuPradziosLikutis { get; set; }
        public virtual decimal PajamosPagaminta { get; set; }
        public virtual decimal PajamosPirkta { get; set; }
        public virtual decimal PajamosImportuota { get; set; }
        public virtual decimal IslaidosVisos { get; set; }
        public virtual decimal IslaidosParduota { get; set; }
        public virtual decimal IslaidosPasarui { get; set; }
        public virtual decimal? IslaidosSeklai { get; set; }
        public virtual decimal IslaidosDuotaPerdirbti { get; set; }
        public virtual decimal IslaidosProdukcijosNuostoliai { get; set; }
        public virtual decimal IslaidosKitos { get; set; }
        public virtual decimal MetuPabaigosLikutis { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
