using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Imone
    {
        public virtual int Id { get; set; }
        public virtual long AsmensKodas { get; set; }
        public virtual ICollection<ImonesDuomenys> Duomenys { get; set; }
        public virtual ICollection<Upload> Uploads { get; set; }

        protected virtual Upload CreateUpload(int metai, DateTime data, string bukle)
        {
            var upload = new Upload()
            {
                Metai = metai,
                Data = data,
                Bukle = bukle,
                Bukles = new List<UploadStatus>(),
                Imone = this
            };

            upload.Bukles.Add(new UploadStatus
            {
                Bukle = bukle,
                DataNuo = data,
                Upload = upload,
            });

            Uploads.Add(upload);

            return upload;
        }

        public virtual Upload CreateNepatvirtintasUpload(int metai, DateTime data)
        {
            AtmestiUploaudus(Uploads, metai, data);
            return CreateUpload(metai, data, "Nepatvirtintas");
        }

        public virtual Upload CreateNetinkamasUpload(int metai, DateTime data)
        {
            return CreateUpload(metai, data, "Netinkamas");
        }

        protected virtual void AtmestiUploaudus(IEnumerable<Upload> kandidatai, int metai, DateTime data)
        {
            var uzdaromiUploudai = kandidatai.Where(x => (x.Bukle == "Nepatvirtintas" || x.Bukle == "Patvirtintas") && x.Metai == metai);

            foreach (var uploud in uzdaromiUploudai)
            {
                uploud.Atmesti(null, data);
            }
        }

        public virtual void PatvirtintiUploada(User user, int uploadId, DateTime data)
        {
            var upload = Uploads.First(x => x.Id == uploadId);

            AtmestiUploaudus(Uploads.Where(x => x.Id != uploadId), upload.Metai, data);
            upload.Patvirtinti(user, data);
        }

        public virtual void AtmestiUploada(User user, int uploadId, DateTime data)
        {
            var upload = Uploads.First(x => x.Id == uploadId);
            upload.Atmesti(user, data);
        }

        public virtual void AddImonesDuomenys(Upload upload, ImonesDuomenys imonesDuomenys)
        {
            Duomenys.Add(imonesDuomenys);
            //imonesDuomenys.Imone = this;
            imonesDuomenys.Upload = upload;
        }
    }

    public class ImonesDuomenys
    {
        public virtual int Id { get; set; }
       // public virtual Imone Imone { get; set; }
        public virtual string Pavadinimas { get; set; }
        public virtual string FormuPildymoData { get; set; }
        public virtual string ImonesVadovas { get; set; }
        public virtual string ImonesFinansininkas { get; set; }
        public virtual string Adresas { get; set; }
        public virtual string ElPastas { get; set; }
        public virtual string Telefonas { get; set; }
        public virtual int AtaskaitosDuomenys { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
