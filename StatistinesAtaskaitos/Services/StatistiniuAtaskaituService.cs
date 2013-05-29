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
        private static readonly Dictionary<KlaidosKodas, string> KlaiduPranesimai = new Dictionary<KlaidosKodas, string>();
        private static readonly Dictionary<FormosTipas, FormuTipai> FormosTipasMap = new Dictionary<FormosTipas, FormuTipai>();

        static StatistiniuAtaskaituService()
        {
            KlaiduPranesimai[KlaidosKodas.F01K01] = "Neigiamas reikšmes gali įgyti tik šios formos 4 ir 12 stulpeliai.";
            KlaiduPranesimai[KlaidosKodas.F01K02] = "3 stulpelio reikšmės negali būti didesnės nei 2 stulpelio reikšmės atitinkamose eilutėse.";
            KlaiduPranesimai[KlaidosKodas.F01K03] = "5 stulpelio reikšmės negali būti mažesnės nei 6 ir 7 stulpelių suma.";
            KlaiduPranesimai[KlaidosKodas.F01K04] = "13 stulpelio reikšmė negali būti mažesnė nei 14 stulpelio reikšmė";
            KlaiduPranesimai[KlaidosKodas.F01K05] = "Kodo 010 reikšmė negali būti mažesnė nei kodų 011 ir 012 suma.";
            KlaiduPranesimai[KlaidosKodas.F01K06] = "Kodo 020 reikšmė negali būti mažesnė nei kodų 021,022,023,024,025,026,027,028 suma.";
            KlaiduPranesimai[KlaidosKodas.F01K07] = "Kodo 030 reikšmė negali būti mažesnė nei kodų 031 ir 032 suma.";
            KlaiduPranesimai[KlaidosKodas.F02K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F02K02] = "Kodo 020 reikšmė visada turi būti mažesnė už kodo 030 reikšmę.";
            KlaiduPranesimai[KlaidosKodas.F02K03] = "Kodo 040 reikšmė negali būti mažesnė nei kodų 041, 042, 043 reikšmių suma.";
            KlaiduPranesimai[KlaidosKodas.F02K04] = "Kodo 010 reikšmė negali būti nulis.";
            KlaiduPranesimai[KlaidosKodas.F02K05] = "Vidutinė darbo dienos trukmė negali būti ilgesnė nei 11 valandų.";
            KlaiduPranesimai[KlaidosKodas.F03K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F03K02] = "1 stulpelio reikšmės turi būti lygios 2 ir 3 stulpelių reikšmių sumai.";
            KlaiduPranesimai[KlaidosKodas.F03K03] = "Kodo 010 reikšmė negali būti mažesnė už kodo 011 reikšmę.";
            KlaiduPranesimai[KlaidosKodas.F03K04] = "Kodo 020 reikšmė negali būti mažesnė už kodų 021, 022, 023, 024, 025, 026, 027, 028, 030, 031, 032 reikšmių sumą.";
            KlaiduPranesimai[KlaidosKodas.F03K05] = "Kodo 021 reikšmė negali būti mažesnė už kodo 021.1 reikšmę.";
            KlaiduPranesimai[KlaidosKodas.F03K06] = "Kodo 022 reikšmė negali būti mažesnė už kodo 022.1 reikšmę.";
            KlaiduPranesimai[KlaidosKodas.F03K07] = "Kodo 024 reikšmė negali būti mažesnė už kodo 024.1 reikšmę.";
            KlaiduPranesimai[KlaidosKodas.F03K08] = "Kodo 050 reikšmė negali būti mažesnė už kodų 052, 053, 054, 055, 056, 057, 058 reikšmių sumą.";
            KlaiduPranesimai[KlaidosKodas.F03K09] = "Kodo 057 reikšmė negali būti mažesnė už kodų 057.1, 057.2,  057.3 reikšmių sumą.";
            KlaiduPranesimai[KlaidosKodas.F41K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F41K02] = "1 stulpelio reikšmės negali būti mažesnės už 2 stulpelio reikšmes.";
            KlaiduPranesimai[KlaidosKodas.F41K03] = "1 stulpelio reiškmės išskyrus kodus 110, 330, 340, 360 negali būti mažesnės už 3 stulpelio reikšmes.";
            KlaiduPranesimai[KlaidosKodas.F41K04] = "1 stulpelio 110 kodo reikšmė turi būti lygi 3 stulpelio 110 kodo reikšmei.";
            KlaiduPranesimai[KlaidosKodas.F41K05] = "1 stulpelio 330 kodo reikšmė turi būti lygi 3 stulpelio 330 kodo reikšmei.";
            KlaiduPranesimai[KlaidosKodas.F41K06] = "1 stulpelio 340 kodo reikšmė turi būti mažesnė nei  3 stulpelio 340 kodo reikšmė.";
            KlaiduPranesimai[KlaidosKodas.F41K07] = "1 stulpelio 360 kodo reikšmė turi būti lygi 3 stulpelio 360 kodo reikšmei.";
            KlaiduPranesimai[KlaidosKodas.F41K08] = "Kodo 020 reikšmė turi būti mažesnė nei kodų 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 310, 320 reikšmių suma.";
            KlaiduPranesimai[KlaidosKodas.F42K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F42K02] = "Kodo 010 reikšmė negali būti mažesnė nei kodų 011, 012, 013 reikšmių suma.";
            KlaiduPranesimai[KlaidosKodas.Fplk01] = "Formos pildymo laikas negali būti neigiamas ir lygus nuliui.";
            KlaiduPranesimai[KlaidosKodas.F05K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F05K02] = "10 stulpelio reikšmės negali būti mažesnės nei 11 stulpelio reikšmės.";
            KlaiduPranesimai[KlaidosKodas.F06K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F07K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F07K02] = "Kodo 010 reikšmė negali būti mažesnė nei kodų 011, 012, 013, 014, 015, 016, 017, 018, 019, 020, 021, 022 reikšmių suma.";
            KlaiduPranesimai[KlaidosKodas.F08K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F08K02] = "Kodo 010 reikšmė negali būti mažesnės nei kodų 011, 012, 013 reikšmių suma.";
            KlaiduPranesimai[KlaidosKodas.F08K03] = "Kodo 020 reikšmė negali būti mažesnė nei kodo 021 reikšmė.";
            KlaiduPranesimai[KlaidosKodas.F08K04] = "Kodo 030 reikšmė negali būti mažesnė nei kodo 031 reikšmė.";
            KlaiduPranesimai[KlaidosKodas.F08K05] = "Kodo 040 reikšmė negali būti mažesnė nei kodo 041 reikšmė.";
            KlaiduPranesimai[KlaidosKodas.F08K06] = "Kodo 050 reikšmė negali būti mažesnė nei kodo 051 reikšmė.";
            KlaiduPranesimai[KlaidosKodas.F09K01] = "Šioje formoje neigiamų reikšmių būti negali.";
            KlaiduPranesimai[KlaidosKodas.F09K02] = "Kodo 010 reikšmė negali būti mažesnė nei kodū 090 ir 100 reikšmių suma.";
            KlaiduPranesimai[KlaidosKodas.F09K03] = "Kodo 020 reikšmė negali būti mažesnė nei kodų 030, 040, 060 reikšmių sumą.";
            KlaiduPranesimai[KlaidosKodas.F09K04] = "Kodo 040 reikšmė negali būti mažesnė nei kodo 050 reikšmė.";

            FormosTipasMap[FormosTipas.IlgalaikisTurtas] = FormuTipai.Forma1;
            FormosTipasMap[FormosTipas.Darbuotojai] = FormuTipai.Forma2;
            FormosTipasMap[FormosTipas.Sanaudos] = FormuTipai.Forma3;
            FormosTipasMap[FormosTipas.ProduktuPardavimas] = FormuTipai.Forma41;
            FormosTipasMap[FormosTipas.DotacijosSubsidijos] = FormuTipai.Forma42;
            FormosTipasMap[FormosTipas.FormosPildymoLaikas] = FormuTipai.Forma42;
            FormosTipasMap[FormosTipas.Augalininkyste] = FormuTipai.Forma5;
            FormosTipasMap[FormosTipas.Gyvulininkyste] = FormuTipai.Forma6;
            FormosTipasMap[FormosTipas.ProdukcijosKaita] = FormuTipai.Forma7;
            FormosTipasMap[FormosTipas.GyvuliuSkaicius] = FormuTipai.Forma8;
            FormosTipasMap[FormosTipas.ZemesPlotai] = FormuTipai.Forma9;
        }

        private readonly ISessionFactory _sessionFactory;

        public StatistiniuAtaskaituService(ISessionFactory sessionFactory)
        {
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

        private List<FormosKlaida> GetKlaidos(ISession session, int? uploadId, FormosTipas formosTipas)
        {
            if (!uploadId.HasValue) return new List<FormosKlaida>();

            return session.Query<KlaidosAprasas>()
                .Where(x => x.FormosTipas == formosTipas && x.Upload.Id == uploadId.Value)
                .AsEnumerable()
                .Select(x => new FormosKlaida
                             {
                                 IrasoKodas = x.IrasoKodas,
                                 Stulpelis = x.Stulpelis,
                                 KlaidosPranesimas = KlaiduPranesimai[x.KlaidosKodas]
                             })
                .ToList();
        }

        public AtaskaitaModel GetStatistineAtaskaita(AtaskaitosParametrai parametrai)
        {
            object rezultatai = null;

            switch (parametrai.FormosTipas)
            {
                case FormuTipai.Forma1:
                    rezultatai = GetForma1(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma2:
                    rezultatai = GetForma2(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);//////////////////////////////////////////
                    break;
                case FormuTipai.Forma3:
                    rezultatai = GetForma3(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma41:
                    rezultatai = GetForma41(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma42:
                    var forma42 = GetForma42(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    var pildymolaikas = GetFormosPildymoLaikas(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);

                    rezultatai = new Forma42IrFormuPildymoLaikas()
                    {
                        Forma42 = forma42,
                        FormosPildymoLaikas = pildymolaikas,
                    };
                    break;
                case FormuTipai.Forma5:
                    rezultatai = GetForma5(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma6:
                    rezultatai = GetForma6(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma7:
                    rezultatai = GetForma7(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma8:
                    rezultatai = GetForma8(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
                case FormuTipai.Forma9:
                    rezultatai = GetForma9(parametrai.Metai, parametrai.ImonesKodas, parametrai.UploadId);
                    break;
            }

            return new AtaskaitaModel
                   {
                       Parametrai = parametrai,
                       Rezultatai = rezultatai,
                       KlaiduSkaicius = GetTotalKlaidos(parametrai.UploadId)
                   };
        }

        private IDictionary<FormuTipai, int> GetTotalKlaidos(int? uploadId)
        {
            if (uploadId == null) return new DefaultableDictionary<FormuTipai, int>(new Dictionary<FormuTipai, int>(), 0);

            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = session.Query<KlaidosAprasas>()
                    .Where(x => x.Upload.Id == uploadId)
                    .GroupBy(x => x.FormosTipas)
                    .Select(x => new
                                 {
                                     Forma = x.Key,
                                     KlaiduSkaicius = x.Count()
                                 }).AsEnumerable();

                var klaidosDictionary = klaidos.ToDictionary(x => FormosTipasMap[x.Forma], x => x.KlaiduSkaicius);

                return new DefaultableDictionary<FormuTipai, int>(klaidosDictionary, 0);
            }
        }

        public IEnumerable<Forma1> GetForma1(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.IlgalaikisTurtas);

                var turtoIrasai = GetStatistineAtaskaita<IlgalaikisTurtas>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma1
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        LikutisPradziojeIlgalaikio = groupResults.Sum(x => x.LikutisPradziojeIlgalaikio) / 1000,
                        Gauta = groupResults.Sum(x => x.Gauta) / 1000,
                        IsJuNauju = groupResults.Sum(x => x.IsJuNauju) / 1000,
                        VertesPadidejimas = groupResults.Sum(x => x.VertesPadidejimas) / 1000,
                        NurasytaIlgalaikio = groupResults.Sum(x => x.NurasytaIlgalaikio) / 1000,
                        LikviduotaIlgalaikio = groupResults.Sum(x => x.LikviduotaIlgalaikio) / 1000,
                        ParduotaIlgalaikio = groupResults.Sum(x => x.ParduotaIlgalaikio) / 1000,
                        Nukainota = groupResults.Sum(x => x.Nukainota) / 1000,
                        LikutisPabaigojeIlgalaikio = groupResults.Sum(x => x.LikutisPabaigojeIlgalaikio) / 1000,
                        LikutisPradziojeNusidevejimo = groupResults.Sum(x => x.LikutisPradziojeNusidevejimo) / 1000,
                        Priskaiciuota = groupResults.Sum(x => x.Priskaiciuota) / 1000,
                        Pasikeitimas = groupResults.Sum(x => x.Pasikeitimas) / 1000,
                        NurasytaNusidevejimo = groupResults.Sum(x => x.NurasytaNusidevejimo) / 1000,
                        LikviduotaNusidevejimo = groupResults.Sum(x => x.LikviduotaNusidevejimo) / 1000,
                        LikutisPabaigojeNusidevejimo = groupResults.Sum(x => x.LikutisPabaigojeNusidevejimo) / 1000
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in turtoIrasai)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());

                    //if (imonesKodas == null && uploadId == null)
                    //{
                    //    irasas.LikutisPradziojeIlgalaikio = irasas.LikutisPabaigojeIlgalaikio / 1000;
                    //    irasas.Gauta = irasas.Gauta / 1000;
                    //    irasas.IsJuNauju = irasas.IsJuNauju/1000;
                    //    irasas.VertesPadidejimas = irasas.VertesPadidejimas / 1000;
                    //    irasas.NurasytaIlgalaikio = irasas.NurasytaIlgalaikio / 1000;
                    //    irasas.LikviduotaIlgalaikio = irasas.LikviduotaIlgalaikio / 1000;
                    //    irasas.ParduotaIlgalaikio = irasas.ParduotaIlgalaikio / 1000;
                    //    irasas.Nukainota = irasas.Nukainota / 1000;
                    //    irasas.LikutisPabaigojeIlgalaikio = irasas.LikutisPabaigojeIlgalaikio / 1000;
                    //    irasas.LikutisPradziojeNusidevejimo = irasas.LikutisPradziojeNusidevejimo / 1000;
                    //    irasas.Priskaiciuota = irasas.Priskaiciuota / 1000;
                    //    irasas.Pasikeitimas = irasas.Pasikeitimas / 1000;
                    //    irasas.NurasytaNusidevejimo = irasas.NurasytaNusidevejimo / 1000;
                    //    irasas.LikviduotaNusidevejimo = irasas.LikviduotaNusidevejimo / 1000;
                    //    irasas.LikutisPabaigojeNusidevejimo = irasas.LikutisPabaigojeNusidevejimo / 1000;
                    //}
                }

                return turtoIrasai.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma2> GetForma2(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.Darbuotojai);

                var darbuotojai = GetStatistineAtaskaita<Darbuotojai>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma2
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Kiekis = groupResults.Sum(x => x.Reiksme),
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in darbuotojai)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());
                    if (irasas.Kodas == "040") irasas.Kiekis = irasas.Kiekis / 1000;
                    if (irasas.Kodas == "041") irasas.Kiekis = irasas.Kiekis / 1000;
                    if (irasas.Kodas == "042") irasas.Kiekis = irasas.Kiekis / 1000;
                    if (irasas.Kodas == "043") irasas.Kiekis = irasas.Kiekis / 1000;
                    if (irasas.Kodas == "044") irasas.Kiekis = irasas.Kiekis / 1000;
                }

                return darbuotojai.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma3> GetForma3(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.Sanaudos);

                var sanaudos = GetStatistineAtaskaita<Sanaudos>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma3
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        IsViso = groupResults.Sum(x => x.IsViso) / 1000,
                        Augalininkyste = groupResults.Sum(x => x.Augalininkyste) / 1000,
                        Gyvulininkyste = groupResults.Sum(x => x.Gyvulininkyste) / 1000,
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in sanaudos)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());

                    //if (imonesKodas == null && uploadId == null)
                    //{
                    //    irasas.IsViso = irasas.IsViso / 1000;
                    //    irasas.Augalininkyste = irasas.Augalininkyste / 1000;
                    //    irasas.Gyvulininkyste = irasas.Gyvulininkyste / 1000;
                    //}
                }

                return sanaudos.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma41> GetForma41(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.ProduktuPardavimas);

                var produktupardavimas = GetStatistineAtaskaita<ProduktuPardavimas>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma41
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        ParduotaNatura =  groupResults.Sum(x => x.ParduotaNatura),
                        ParduotaEksportui = groupResults.Sum(x => x.ParduotaEksportui),
                        ParduotaIskaitomuojuSvoriu = groupResults.Sum(x => x.ParduotaIskaitomuojuSvoriu),
                        ProdukcijosSavikaina = groupResults.Sum(x => x.ProdukcijosSavikaina)/1000,
                        PardavimuPajamos = groupResults.Sum(x => x.PardavimuPajamos)/1000,
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in produktupardavimas)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    
                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());

                }

                return produktupardavimas.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma42> GetForma42(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.DotacijosSubsidijos);

                var dotacijossubsidijos = GetStatistineAtaskaita<DotacijosSubsidijos>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma42
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Suma = groupResults.Sum(x => x.Suma)/1000,
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in dotacijossubsidijos)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());
                }

                return dotacijossubsidijos.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma5> GetForma5(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.Augalininkyste);

                var augalininkyste = GetStatistineAtaskaita<Augalininkyste>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma5
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Plotas = groupResults.Sum(x => x.Plotas),
                        ProdukcijosKiekis = groupResults.Sum(x => x.ProdukcijosKiekis),
                        IslaidosDarboApmokejimas =  groupResults.Sum(x => x.IslaidosDarboApmokejimas)/1000,
                        IslaidosSeklos = groupResults.Sum(x => x.IslaidosSeklos) / 1000,
                        IslaidosTrasos = groupResults.Sum(x => x.IslaidosTrasos) / 1000,
                        IslaidosNafta = groupResults.Sum(x => x.IslaidosNafta) / 1000,
                        IslaidosElektra = groupResults.Sum(x => x.IslaidosElektra) / 1000,
                        IslaidosKitos = groupResults.Sum(x => x.IslaidosElektra) / 1000,
                        IslaidosVisos = groupResults.Sum(x => x.IslaidosDarboApmokejimas + x.IslaidosSeklos + x.IslaidosTrasos + x.IslaidosNafta + x.IslaidosElektra + x.IslaidosKitos) / 1000,
                        IslaidosPagrindinei = groupResults.Sum(x => x.IslaidosPagrindinei) / 1000,
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in augalininkyste)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());

                    irasas.ProdukcijosSavikaina = irasas.ProdukcijosKiekis == 0 ? 0 : irasas.IslaidosPagrindinei/irasas.ProdukcijosKiekis*1000; //---------!!!!!!!!!!!
                    irasas.Derlingumas = irasas.Plotas == 0 ? 0 : irasas.ProdukcijosKiekis/irasas.Plotas*10;
                }

                return augalininkyste.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma6> GetForma6(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.Gyvulininkyste);

                var gyvulininkyste = GetStatistineAtaskaita<Gyvulininkyste>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma6
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        VidutinisGyvuliuSk = groupResults.Sum(x => x.VidutinisGyvuliuSk),
                        ProdukcijosKiekis = groupResults.Sum(x => x.ProdukcijosKiekis),
                        IslaidosDarboApmokejimas = groupResults.Sum(x => x.IslaidosDarboApmokejimas)/1000,
                        IslaidosPasarai = groupResults.Sum(x => x.IslaidosPasarai) / 1000,
                        IslaidosNafta = groupResults.Sum(x => x.IslaidosNafta) / 1000,
                        IslaidosElektra = groupResults.Sum(x => x.IslaidosElektra) / 1000,
                        IslaidosKitos = groupResults.Sum(x => x.IslaidosKitos) / 1000,
                        IslaidosVisos = groupResults.Sum(x => x.IslaidosVisos) / 1000,
                        IslaidosPagrindinei = groupResults.Sum(x => x.IslaidosPagrindinei) / 1000,
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in gyvulininkyste)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());
                    irasas.ProdukcijosSavikaina = irasas.ProdukcijosKiekis == 0 ? 0 : irasas.IslaidosPagrindinei / irasas.ProdukcijosKiekis*1000;
                }

                return gyvulininkyste.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma7> GetForma7(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.ProdukcijosKaita);

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
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in produkcijoskaita)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());
                }

                return produkcijoskaita.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma8> GetForma8(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.GyvuliuSkaicius);

                var gyvuliuskaicius = GetStatistineAtaskaita<GyvuliuSkaicius>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma8
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        MetuPradzioje = groupResults.Sum(x => x.MetuPradzioje),
                        MetuPabaigojeVnt = groupResults.Sum(x => x.MetuPabaigojeVnt),
                        MetuPabaigojeVerte = groupResults.Sum(x => x.MetuPabaigojeVerte)/1000,
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in gyvuliuskaicius)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());
                }

                return gyvuliuskaicius.ToList().OrderBy(x => x.Kodas);
            }
        }

        public IEnumerable<Forma9> GetForma9(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.ZemesPlotai);

                var zemesplotai = GetStatistineAtaskaita<ZemesPlotai>(session, metai, imonesKodas, uploadId)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma9
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        NuomaIsValstybes = groupResults.Sum(x => x.NuomaIsValstybes),
                        NuomaIsFiziniu = groupResults.Sum(x => x.NuomaIsFiziniu),
                        NuosavaZeme = groupResults.Sum(x => x.NuosavaZeme),
                    }).ToList();

                var indexedKlaidos = klaidos.GroupBy(x => x.IrasoKodas)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var irasas in zemesplotai)
                {
                    List<FormosKlaida> eilutesKlaidos;
                    if (!indexedKlaidos.TryGetValue(irasas.Kodas, out eilutesKlaidos)) eilutesKlaidos = new List<FormosKlaida>();

                    irasas.Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(eilutesKlaidos.GroupBy(x => x.Stulpelis)
                        .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>());
                }

                return zemesplotai.ToList().OrderBy(x => x.Kodas);
            }
        }

        public FormosPildymoLaikasModel GetFormosPildymoLaikas(int? metai, long? imonesKodas, int? uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var klaidos = GetKlaidos(session, uploadId, FormosTipas.FormosPildymoLaikas);

                var pildymoLaikas = GetStatistineAtaskaita<Vic.ZubStatistika.Entities.FormosPildymoLaikas>(session, metai, imonesKodas, uploadId)
                    .AsEnumerable();

                    return new FormosPildymoLaikasModel
                                     {
                                         Valandos = (int) ((pildymoLaikas.Average(x => x.Valandos*60) + pildymoLaikas.Average(x => x.Minutes))/60),
                                         Minutes = (int)((pildymoLaikas.Average(x => x.Valandos * 60) + pildymoLaikas.Average(x => x.Minutes)) % 60),
                                         Klaidos = new DefaultableDictionary<int, List<FormosKlaida>>(klaidos.GroupBy(x => x.Stulpelis)
                                         .ToDictionary(x => x.Key, x => x.ToList()), new List<FormosKlaida>())
                                     };
            }
        }
    }
}