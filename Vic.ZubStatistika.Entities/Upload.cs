using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Upload
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual ICollection<UploadStatus> Bukles { get; set; }
        public virtual int Metai { get; set; }
        public virtual DateTime Data { get; set; }
        public virtual string Bukle { get; set; }
        public virtual string FileId { get; set; }
        public virtual ICollection<IlgalaikisTurtas> IlgalaikisTurtas { get; set; }
        public virtual ICollection<ImonesDuomenys> ImonesDuomenys { get; set; }
        public virtual ICollection<Augalininkyste> Augalininkyste { get; set; }
        public virtual ICollection<Darbuotojai> Darbuotojai { get; set; }
        public virtual ICollection<DotacijosSubsidijos> DotacijosSubsidijos { get; set; }
        public virtual ICollection<FormosPildymoLaikas> FormosPildymoLaikas { get; set; }
        public virtual ICollection<Gyvulininkyste> Gyvulininkyste { get; set; }
        public virtual ICollection<GyvuliuSkaicius> GyvuliuSkaicius { get; set; }
        public virtual ICollection<ProdukcijosKaita> ProdukcijosKaita { get; set; }
        public virtual ICollection<ProduktuPardavimas> ProduktuPardavimas { get; set; }
        public virtual ICollection<Sanaudos> Sanaudos { get; set; }
        public virtual ICollection<ZemesPlotai> ZemesPlotai { get; set; }
        public virtual ICollection<KlaidosAprasas> Klaidos { get; set; }

        protected virtual void CreateStatus(User user, string bukle, DateTime dataNuo)
        {
            foreach (var oldStatus in Bukles.Where(x => x.DataIki == null))
            {
                oldStatus.DataIki = dataNuo;
            }

            var status = new UploadStatus
                             {
                                 Upload = this,
                                 User = user,
                                 Bukle = bukle,
                                 DataNuo = dataNuo
                             };

            Bukles.Add(status);
            Bukle = bukle;
        }

        public virtual void Patvirtinti(User user, DateTime data)
        {
            CreateStatus(user, "Patvirtintas", data);
        }

        public virtual void Atmesti(User user, DateTime data)
        {
            CreateStatus(user, "Atmestas", data);
        }

    }
}
