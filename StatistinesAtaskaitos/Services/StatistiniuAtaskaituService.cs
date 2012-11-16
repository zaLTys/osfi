using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using Oracle.DataAccess.Client;
using PetaPoco;
using StatistinesAtaskaitos.Models;
using Vic.ZubStatistika.Entities;
using FormosPildymoLaikas = StatistinesAtaskaitos.Models.FormosPildymoLaikasModel;

namespace StatistinesAtaskaitos.Services
{
    public class StatistiniuAtaskaituService : IStatistiniuAtaskaituService
    {
        private readonly PetaPoco.Database _db;
        private readonly ISessionFactory _sessionFactory;

        public StatistiniuAtaskaituService(Database db, ISessionFactory sessionFactory)
        {
            _db = db;
            _sessionFactory = sessionFactory;
        }


        private IQueryable<TAtaskaita> GetStatistineAtaskaita<TAtaskaita>(ISession session, int? metai, long? imonesKodas, int? uploadId)
            where TAtaskaita : IStatistineAtaskaita
        {
            var ataskaitosIrasai = session.Query<TAtaskaita>();

            if (imonesKodas.HasValue) ataskaitosIrasai = ataskaitosIrasai.Where(x => x.Upload.Imone.AsmensKodas == imonesKodas.Value);

            if (uploadId.HasValue)
            {
                ataskaitosIrasai = ataskaitosIrasai.Where(x => x.Upload.Id == uploadId.Value);
            }
            else
            {
                ataskaitosIrasai = ataskaitosIrasai.Where(x => x.Upload.Metai == metai && x.Upload.Bukle == "Patvirtintas");
            }

            return ataskaitosIrasai;
        }

        public IEnumerable<Forma1> GetForma1(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var turtoIrasai = GetStatistineAtaskaita<IlgalaikisTurtas>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma1
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        LikutisPradziojeIlgalaikio = groupResults.Sum(x => x.LikutisPradziojeIlgalaikio),
                        Gauta = groupResults.Sum(x => x.Gauta),
                        IsJuNauju = groupResults.Sum(x => x.IsJuNauju),
                        VertesPadidejimas = groupResults.Sum(x => x.VertesPadidejimas),
                        NurasytaIlgalaikio = groupResults.Sum(x => x.NurasytaIlgalaikio),
                        LikviduotaIlgalaikio = groupResults.Sum(x => x.LikviduotaIlgalaikio),
                        ParduotaIlgalaikio = groupResults.Sum(x => x.ParduotaIlgalaikio),
                        Nukainota = groupResults.Sum(x => x.Nukainota),
                        LikutisPabaigojeIlgalaikio = groupResults.Sum(x => x.LikutisPabaigojeIlgalaikio),
                        LikutisPradziojeNusidevejimo = groupResults.Sum(x => x.LikutisPradziojeNusidevejimo),
                        Priskaiciuota = groupResults.Sum(x => x.Priskaiciuota),
                        Pasikeitimas = groupResults.Sum(x => x.Pasikeitimas),
                        NurasytaNusidevejimo = groupResults.Sum(x => x.NurasytaNusidevejimo),
                        LikviduotaNusidevejimo = groupResults.Sum(x => x.LikviduotaNusidevejimo),
                        LikutisPabaigojeNusidevejimo = groupResults.Sum(x => x.LikutisPabaigojeNusidevejimo)
                    });

                return turtoIrasai.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma2> GetForma2(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var darbuotojai = GetStatistineAtaskaita<Darbuotojai>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma2
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Kiekis = groupResults.Sum(x => x.Reiksme)    
                    });

                return darbuotojai.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma3> GetForma3(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var sanaudos = GetStatistineAtaskaita<Sanaudos>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma3
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        IsViso = groupResults.Sum(x => x.IsViso),
                        Augalininkyste = groupResults.Sum(x => x.Augalininkyste),
                        Gyvulininkyste = groupResults.Sum(x => x.Gyvulininkyste)
                    });

                return sanaudos.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma41> GetForma41(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var produktupardavimas = GetStatistineAtaskaita<ProduktuPardavimas>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma41
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        ParduotaNatura =  groupResults.Sum(x => x.ParduotaNatura),
                        ParduotaEksportui = groupResults.Sum(x => x.ParduotaEksportui),
                        ParduotaIskaitomuojuSvoriu = groupResults.Sum(x => x.ParduotaIskaitomuojuSvoriu),
                        PardavimuPajamos = groupResults.Sum(x => x.PardavimuPajamos)
                    });

                return produktupardavimas.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma42> GetForma42(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var dotacijossubsidijos = GetStatistineAtaskaita<DotacijosSubsidijos>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma42
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Suma = groupResults.Sum(x => x.Suma)
                    });

                return dotacijossubsidijos.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma5> GetForma5(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var augalininkyste = GetStatistineAtaskaita<Augalininkyste>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma5
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Plotas = groupResults.Sum(x => x.Plotas),
                        ProdukcijosKiekis = groupResults.Sum(x => x.ProdukcijosKiekis),
                        Derlingumas = groupResults.Sum(x => x.Derlingumas),
                        IslaidosDarboApmokejimas =  groupResults.Sum(x => x.IslaidosDarboApmokejimas),
                        IslaidosSeklos =  groupResults.Sum(x => x.IslaidosSeklos),
                        IslaidosTrasos =  groupResults.Sum(x => x.IslaidosTrasos),
                        IslaidosNafta = groupResults.Sum(x => x.IslaidosNafta),
                        IslaidosElektra = groupResults.Sum(x => x.IslaidosElektra),
                        IslaidosKitos = groupResults.Sum(x => x.IslaidosElektra),
                        IslaidosVisos = groupResults.Sum(x => x.IslaidosDarboApmokejimas + x.IslaidosSeklos + x.IslaidosTrasos+ x.IslaidosNafta +x.IslaidosElektra + x.IslaidosKitos),
                        IslaidosPagrindinei = groupResults.Sum(x => x.IslaidosPagrindinei),
                        ProdukcijosSavikaina = groupResults.Sum(x => x.ProdukcijosSavikaina)
                    });

                return augalininkyste.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma6> GetForma6(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var gyvulininkyste = GetStatistineAtaskaita<Gyvulininkyste>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma6
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        VidutinisGyvuliuSk = groupResults.Sum(x => x.VidutinisGyvuliuSk),
                        ProdukcijosKiekis = groupResults.Sum(x => x.ProdukcijosKiekis),
                        IslaidosDarboApmokejimas = groupResults.Sum(x => x.IslaidosDarboApmokejimas),
                        IslaidosPasarai = groupResults.Sum(x => x.IslaidosPasarai),
                        IslaidosNafta = groupResults.Sum(x => x.IslaidosNafta),
                        IslaidosElektra = groupResults.Sum(x => x.IslaidosElektra),
                        IslaidosKitos = groupResults.Sum(x => x.IslaidosKitos),
                        IslaidosVisos = groupResults.Sum(x => x.IslaidosVisos),
                        IslaidosPagrindinei = groupResults.Sum(x => x.IslaidosPagrindinei),
                        ProdukcijosSavikaina = groupResults.Sum(x => x.ProdukcijosSavikaina),
                    
                    });

                return gyvulininkyste.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma7> GetForma7(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var produkcijoskaita = GetStatistineAtaskaita<ProdukcijosKaita>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma7
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        MetuPradziosLikutis = groupResults.Sum(x => x.MetuPradziosLikutis),
                        PajamosPagaminta = groupResults.Sum(x => x.PajamosPagaminta),
                        PajamosPirkta = groupResults.Sum(x => x.PajamosPirkta),
                        PajamosImportuota = groupResults.Sum(x => x.PajamosImportuota),
                        IslaidosVisos = groupResults.Sum(x => x.IslaidosVisos),
                        IslaidosParduota = groupResults.Sum(x => x.IslaidosParduota),
                        IslaidosPasarui = groupResults.Sum(x => x.IslaidosPasarui),
                        IslaidosSeklai = groupResults.Sum(x => x.IslaidosSeklai),
                        IslaidosDuotaPerdirbti = groupResults.Sum(x => x.IslaidosDuotaPerdirbti),
                        IslaidosProdukcijosNuostoliai = groupResults.Sum(x => x.IslaidosProdukcijosNuostoliai),
                        IslaidosKitos = groupResults.Sum(x => x.IslaidosKitos),
                        MetupabaigosLikutis = groupResults.Sum(x => x.MetuPabaigosLikutis), //FAK JU UNCA DOLAN
                    });

                return produkcijoskaita.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma8> GetForma8(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var gyvuliuskaicius = GetStatistineAtaskaita<GyvuliuSkaicius>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma8
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        MetuPradzioje = groupResults.Sum(x => x.MetuPradzioje),
                        MetuPabaigojeVnt = groupResults.Sum(x => x.MetuPabaigojeVnt),
                        MetuPabaigojeVerte = groupResults.Sum(x => x.MetuPabaigojeVerte),
                    });

                return gyvuliuskaicius.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma9> GetForma9(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var zemesplotai = GetStatistineAtaskaita<ZemesPlotai>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma9
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        NuomaIsValstybes = groupResults.Sum(x => x.NuomaIsValstybes),
                        NuomaIsFiziniu = groupResults.Sum(x => x.NuomaIsFiziniu),
                        NuosavaZeme = groupResults.Sum(x => x.NuosavaZeme),
                        /////PABAIGT

                    });

                return zemesplotai.ToList().OrderBy(x => x.Kodas);
            }
        }

        public FormosPildymoLaikasModel GetFormosPildymoLaikas(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var pildymoLaikas = GetStatistineAtaskaita<Vic.ZubStatistika.Entities.FormosPildymoLaikas>(session, metai, imonesKodas, uploadId)
                    .AsEnumerable();

                    return new FormosPildymoLaikasModel
                                     {
                                         Valandos = (int) ((pildymoLaikas.Average(x => x.Valandos*60) + pildymoLaikas.Average(x => x.Minutes))/60),
                                         Minutes = (int) ((pildymoLaikas.Average(x => x.Valandos*60) + pildymoLaikas.Average(x => x.Minutes))%60)
                                     };
            }
        }
    }
}