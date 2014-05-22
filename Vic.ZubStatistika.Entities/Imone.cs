using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Imone()
        {
            Duomenys = new Collection<ImonesDuomenys>();
            Uploads = new Collection<Upload>();
        }

        public virtual Upload CreateUpload(
            int metai, 
            Guid fileId,
            DateTime data, 
            IEnumerable<IlgalaikisTurtas> ilgalaikisTurtas,
            ImonesDuomenys imonesDuomenys,
            IEnumerable<Augalininkyste> augalininkyste,
            IEnumerable<Darbuotojai> darbuotojai,
            IEnumerable<DotacijosSubsidijos> dotacijosSubsidijos,
            FormosPildymoLaikas formosPildymoLaikas,
            IEnumerable<Gyvulininkyste> gyvulininkyste,
            IEnumerable<GyvuliuSkaicius> gyvuliuSkaicius,
            IEnumerable<ProdukcijosKaita> produkcijosKaita,
            IEnumerable<ProduktuPardavimas> produktuPardavimas,
            IEnumerable<Sanaudos> sanaudos,
            IEnumerable<ZemesPlotai> zemesPlotai,
            IEnumerable<Kapitalas> kapitalas,
            IUploadValidator validator)
        {
            Duomenys.Add(imonesDuomenys);

            var upload = new Upload()
                         {
                             Metai = metai,
                             Data = data,
                             FileId = fileId.ToString("N"),
                             Bukles = new List<UploadStatus>(),
                             Imone = this,
                             IlgalaikisTurtas = ilgalaikisTurtas.ToList(),
                             ImonesDuomenys = new List<ImonesDuomenys> {imonesDuomenys},
                             Augalininkyste = augalininkyste.ToList(),
                             Darbuotojai = darbuotojai.ToList(),
                             DotacijosSubsidijos = dotacijosSubsidijos.ToList(),
                             FormosPildymoLaikas = new List<FormosPildymoLaikas> {formosPildymoLaikas},
                             Gyvulininkyste = gyvulininkyste.ToList(),
                             GyvuliuSkaicius = gyvuliuSkaicius.ToList(),
                             ProdukcijosKaita = produkcijosKaita.ToList(),
                             ProduktuPardavimas = produktuPardavimas.ToList(),
                             Sanaudos = sanaudos.ToList(),
                             ZemesPlotai = zemesPlotai.ToList(),
                             Kapitalas = kapitalas.ToList()
                         };

            foreach (var irasas in upload.IlgalaikisTurtas) irasas.Upload = upload;
            foreach (var irasas in upload.ImonesDuomenys) irasas.Upload = upload;
            foreach (var irasas in upload.Augalininkyste) irasas.Upload = upload;
            foreach (var irasas in upload.Darbuotojai) irasas.Upload = upload;
            foreach (var irasas in upload.DotacijosSubsidijos) irasas.Upload = upload;
            foreach (var irasas in upload.FormosPildymoLaikas) irasas.Upload = upload;
            foreach (var irasas in upload.Gyvulininkyste) irasas.Upload = upload;
            foreach (var irasas in upload.GyvuliuSkaicius) irasas.Upload = upload;
            foreach (var irasas in upload.ProdukcijosKaita) irasas.Upload = upload;
            foreach (var irasas in upload.ProduktuPardavimas) irasas.Upload = upload;
            foreach (var irasas in upload.Sanaudos) irasas.Upload = upload;
            foreach (var irasas in upload.ZemesPlotai) irasas.Upload = upload;
            foreach (var irasas in upload.Kapitalas) irasas.Upload = upload;

            var klaidos = validator.Validate(upload).ToArray();

            var bukle = "Nepatvirtintas";
            if (klaidos.Any()) bukle = "Netinkamas";
            upload.Klaidos = klaidos.ToList();

            upload.Bukle = bukle;

            upload.Bukles.Add(new UploadStatus
            {
                DataNuo = data,
                Upload = upload,
                Bukle = bukle
            });

            Uploads.Add(upload);

            return upload;
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
