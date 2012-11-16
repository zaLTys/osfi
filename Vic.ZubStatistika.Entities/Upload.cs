using System;
using System.Collections.Generic;
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
        public virtual ICollection<IlgalaikisTurtas> IlgalaikisTurtas { get; set; }

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
