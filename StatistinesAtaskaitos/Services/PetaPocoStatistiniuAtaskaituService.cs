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
    public class PetaPocoStatistiniuAtaskaituService : IStatistiniuAtaskaituService
    {
        private readonly PetaPoco.Database _db;
        private readonly ISessionFactory _sessionFactory;

        public PetaPocoStatistiniuAtaskaituService(Database db, ISessionFactory sessionFactory)
        {
            _db = db;
            _sessionFactory = sessionFactory;
        }

        
        private IQueryable<TAtaskaita> GetStatistineAtaskaita<TAtaskaita>(ISession session, int metai)
            where TAtaskaita : IStatistineAtaskaita
        {
            var turtoIrasai = session.Query<TAtaskaita>()
                .Join(session.Query<UploadStatus>(), x => x.Upload.Id, x => x.Upload.Id, (ataskaita, bukle) => new { ataskaita, bukle })
                .Where(x => x.ataskaita.Upload.Metai == metai && x.bukle.DataIki == null && x.bukle.Bukle == "Patvirtintas")
                .Select(x => x.ataskaita);

            return turtoIrasai;
        }

        public IEnumerable<Forma1> GetForma1(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var turtoIrasai = GetStatistineAtaskaita<IlgalaikisTurtas>(session, metai)
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

        public IEnumerable<Forma2> GetForma2(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var darbuotojai = GetStatistineAtaskaita<Darbuotojai>(session, metai)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma2
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Kiekis = groupResults.Sum(x => x.Reiksme)    
                    });

                return darbuotojai.ToList().OrderBy(x => x.Kodas);
            }
        }


       /* public IEnumerable<Forma2> GetForma2(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma2>(
                @"select
                    dr.pavadinimas Pavadinimas
                    ,dr.kodas Kodas
                    ,decode(d.rusis_id, 4, sum(d.reiksme/1), ??????????????????????????????
                                        5, sum(d.reiksme/1),
                                        6, sum(d.reiksme/1),
                                        7, sum(d.reiksme/1),
                                        8, sum(d.reiksme/1),
                                        sum(d.reiksme)) Kiekis
                    from
                    osfi.darbuotojai d
                    ,osfi.darbuotojurusis dr
                    ,osfi.imone i
                    where
                    d.rusis_id = dr.id
                    and d.dataiki is null
                    and d.imone_id = i.id
                    and i.asmenskodas in(" + String.Join(",", imoniuKodai) + @") -----
                    
                    group by dr.kodas, dr.pavadinimas, d.rusis_id
                    order by 2");
        }*/


        public IEnumerable<Forma3> GetForma3(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var sanaudos = GetStatistineAtaskaita<Sanaudos>(session, metai)
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
        /*public IEnumerable<Forma3> GetForma3(int metai)
        {
            return _db.Query<Forma3>(
                @"select
                  sr.kodas
                  ,sr.pavadinimas
                  ,sum(s.isviso/1) isviso
                  ,sum(s.augalininkyste/1) augalininkyste
                  ,sum(s.gyvulininkyste/1) gyvulininkyste
                  
                  from
                  osfi.sanaudos s
                  ,osfi.sanaudurusis sr
                  ,osfi.imone im
                  where
                  s.dataiki is null
                  and s.rusis_id =sr.id
                  and im.id = s.imone_id
                  and im.asmenskodas in(" + String.Join(",", imoniuKodai) + @")
                  
                  group by 
                  sr.kodas
                  ,sr.pavadinimas
                  order by 1
                  ");
        }*/
        public IEnumerable<Forma41> GetForma41(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var produktupardavimas = GetStatistineAtaskaita<ProduktuPardavimas>(session, metai)
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
       
        /*public IEnumerable<Forma41> GetForma41(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma41>(
                @"select
                  ppr.kodas
                  ,ppr.pavadinimas
                  ,sum(pp.parduotanatura) parduotanatura
                  ,sum(pp.parduotaeksportui) parduotaeksportui
                  ,sum(pp.parduotaiskaitomuojusvoriu) parduotaiskaitomuojusvoriu
                  ,sum(pp.produkcijossavikaina/1) produkcijossavikaina
                  ,sum(pp.pardavimupajamos/1) pardavimupajamos
                  from
                  osfi.produktupardavimas pp
                  ,osfi.produktupardavimorusis ppr
                  ,osfi.imone i
                  where
                  pp.dataiki is null
                  and pp.rusis_id =ppr.id
                  and pp.imone_id = i.id
                  and i.asmenskodas in (" + String.Join(",", imoniuKodai) + @")
                  group by 
                  ppr.kodas
                  ,ppr.pavadinimas
                  order by 1                 
                  ");
        }*/

        public IEnumerable<Forma42> GetForma42(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var dotacijossubsidijos = GetStatistineAtaskaita<DotacijosSubsidijos>(session, metai)
                    .GroupBy(x => new { x.Rusis.Kodas, x.Rusis.Pavadinimas }, (key, groupResults) => new Forma42
                    {
                        Pavadinimas = key.Pavadinimas,
                        Kodas = key.Kodas,
                        Suma = groupResults.Sum(x => x.Suma)
                    });

                return dotacijossubsidijos.ToList().OrderBy(x => x.Kodas);
            }
        }
        /*public IEnumerable<Forma42> GetForma42(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma42>(
                @"select 
                  dr.kodas,
                  dr.pavadinimas
                  ,sum( d.suma/1) suma
                  from 
                  osfi.dotacijossubsidijos d
                  ,osfi.dotacijusubsidijurusis dr
                  ,osfi.imone im
                  where
                  d.dataiki is null
                  and dr.id = d.rusis_id
                  and d.imone_id = im.id
                  and im.asmenskodas IN (" + String.Join(",", imoniuKodai) + @")
                  group by
                  dr.kodas,
                  dr.pavadinimas
                  order by 1,2                
                  ");
        }*/

        public IEnumerable<Forma5> GetForma5(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var augalininkyste = GetStatistineAtaskaita<Augalininkyste>(session, metai)
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

        /*public IEnumerable<Forma5> GetForma5(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma5>(
                @"select
                  ar.kodas kodas
                  ,ar.pavadinimas pavadinimas
                  ,sum(a.plotas) plotas
                  ,sum(a.produkcijoskiekis) produkcijoskiekis
                  ,round(decode(sum(nvl(a.plotas,0)),0,0,(sum(a.produkcijoskiekis)/sum(nvl(a.plotas,0))*10)),2) derlingumas
                  --,sum(a.derlingumas) derlingumas 
                  ,sum(a.islaidosdarboapmokejimas/1) islaidosdarboapmokejimas
                  ,sum(a.islaidosseklos/1) islaidosseklos
                  ,sum(a.islaidostrasos/1) islaidostrasos
                  ,sum(a.islaidosnafta/1) islaidosnafta
                  ,sum(a.islaidoselektra/1) islaidoselektra
                  ,sum(a.islaidoskitos/1) islaidoskitos
                  ,sum((a.islaidosdarboapmokejimas
                      +a.islaidosseklos
                      +a.islaidostrasos
                      +a.islaidosnafta
                      +a.islaidoselektra
                      +a.islaidoskitos)/1) islaidosvisos
                  ,sum(a.islaidospagrindinei/1) islaidospagrindinei
                  ,sum(a.produkcijossavikaina/1) produkcijossavikaina
                  from
                  osfi.augalininkyste a
                  ,osfi.augalininkystesrusis ar
                  ,osfi.imone im
                  where 
                      a.dataiki is null
                  and ar.id = a.rusis_id
                  and a.imone_id = im.id
                  and im.asmenskodas  in (" + String.Join(",", imoniuKodai) + @")
                  group by 
                  ar.kodas
                  ,ar.pavadinimas
                  order by 1
                  ");
        }*/

        public IEnumerable<Forma6> GetForma6(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var gyvulininkyste = GetStatistineAtaskaita<Gyvulininkyste>(session, metai)
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
        /*public IEnumerable<Forma6> GetForma6(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma6>(
                @"select
                  ar.kodas kodas
                  ,ar.pavadinimas pavadinimas
                  ,decode(a.rusis_id, 12,sum(a.vidutinisgyvuliusk/1000),sum(a.vidutinisgyvuliusk)) vidutinisgyvuliusk
                  -- kiausinius dalint is 1000 ^^^
                  ,sum(a.produkcijoskiekis) produkcijoskiekis
                  ,sum(a.islaidosdarboapmokejimas/1) islaidosdarboapmokejimas
                  ,sum(a.islaidospasarai/1) islaidospasarai
                  ,sum(a.islaidosnafta/1) islaidosnafta
                  ,sum(a.islaidoselektra/1) islaidoselektra
                  ,sum(a.islaidoskitos/1) islaidoskitos
                  ,sum(a.islaidosvisos/1) islaidosvisos
                  ,sum(a.islaidospagrindinei/1) islaidospagrindinei
                  ,sum(a.produkcijossavikaina/1) produkcijossavikaina
                  from
                   osfi.gyvulininkyste a
                  ,osfi.gyvulininkystesrusis ar
                  ,osfi.imone im
                  where 
                      a.dataiki is null
                  and ar.id = a.rusis_id
                  and a.imone_id = im.id
                  and im.asmenskodas in (" + String.Join(",", imoniuKodai) + @")
                  group by 
                  ar.kodas
                  ,ar.pavadinimas
                  ,a.rusis_id
                  order by 1
                  ");
        }*/

        public IEnumerable<Forma7> GetForma7(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var produkcijoskaita = GetStatistineAtaskaita<ProdukcijosKaita>(session, metai)
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

       /* public IEnumerable<Forma7> GetForma7(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma7>(
                @"select
                  ar.kodas kodas
                  ,ar.pavadinimas pavadinimas
                  ,sum(a.metupradzioslikutis) metupradzioslikutis
                  ,sum(a.pajamospagaminta) pajamospagaminta
                  ,sum(a.pajamospirkta) pajamospirkta
                  ,sum(a.pajamosimportuota) pajamosimportuota
                  ,sum(a.islaidosvisos) islaidosvisos
                  ,sum(a.islaidosparduota) islaidosparduota
                  ,sum(a.islaidospasarui) islaidospasarui
                  ,sum(a.islaidosseklai) islaidosseklai
                  ,sum(a.islaidosduotaperdirbti) islaidosduotaperdirbti
                  ,sum(a.islaidosprodukcijosnuostoliai) islaidosprodukcijosnuostoliai
                  ,sum(a.islaidoskitos) islaidoskitos
                  ,sum(a.metupabaigoslikutis) metupabaigoslikutis
                  from
                  osfi.produkcijoskaita a
                  ,osfi.produkcijoskaitosrusis ar
                  ,osfi.imone im
                  where 
                      a.dataiki is null
                  and ar.id = a.rusis_id
                  and im.id = a.imone_id
                  and im.asmenskodas in (" + String.Join(",", imoniuKodai) + @")
                  group by 
                  ar.kodas
                  ,ar.pavadinimas
                  order by 1
                  ");
        }*/

        public IEnumerable<Forma8> GetForma8(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var gyvuliuskaicius = GetStatistineAtaskaita<GyvuliuSkaicius>(session, metai)
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

        /*public IEnumerable<Forma8> GetForma8(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma8>(
                @"select
                  ar.kodas
                  ,ar.pavadinimas
                  ,sum(a.metupradzioje) metupradzioje --SUDET DECODE 040, 041 kodams
                  ,sum(a.metupabaigojevnt) metupabaigojevnt --SUDET DECODE 040, 041 kodams
                  ,sum(a.metupabaigojeverte/1) metupabaigojeverte
                  from
                  osfi.gyvuliuskaicius a
                  ,osfi.gyvuliuskaiciausrusis ar
                  ,osfi.imone im
                  where 
                      a.dataiki is null
                  and ar.id = a.rusis_id
                  and im.id = a.imone_id
                  and im.asmenskodas in (" + String.Join(",", imoniuKodai) + @")
                  group by 
                  ar.kodas
                  ,ar.pavadinimas
                  order by 1
                  ");
        }*/

        public IEnumerable<Forma9> GetForma9(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var zemesplotai = GetStatistineAtaskaita<ZemesPlotai>(session, metai)
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

        /*public IEnumerable<Forma9> GetForma9(IEnumerable<long> imoniuKodai)
        {
            return _db.Query<Forma9>(
                @"select
                  ar.kodas
                  ,ar.pavadinimas
                  ,sum(a.nuomaisvalstybes) nuomaisvalstybes
                  ,sum(a.nuomaisfiziniu) nuomaisfiziniu
                  ,sum(a.nuosavazeme) nuosavazeme
                  from
                  osfi.zemesplotai a
                  ,osfi.zemesploturusis ar
                  ,osfi.imone i
                  where 
                      a.dataiki is null
                  and ar.id = a.rusis_id
                  and i.id = a.imone_id
                 and i.asmenskodas in (" + String.Join(",", imoniuKodai) + @")
                  group by 
                  ar.kodas
                  ,ar.pavadinimas
                  order by 1
                  ");
        }*/

        public FormosPildymoLaikasModel GetFormosPildymoLaikas(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var pildymoLaikas = GetStatistineAtaskaita<Vic.ZubStatistika.Entities.FormosPildymoLaikas>(session, metai)
                    .AsEnumerable();

                    return new FormosPildymoLaikasModel
                                     {
                                         Valandos = (int) ((pildymoLaikas.Average(x => x.Valandos*60) + pildymoLaikas.Average(x => x.Minutes))/60),
                                         Minutes = (int) ((pildymoLaikas.Average(x => x.Valandos*60) + pildymoLaikas.Average(x => x.Minutes))%60)
                                     };
            }
        }
        /*public FormosPildymoLaikas GetFormosPildymoLaikas(IEnumerable<long> imoniuKodai)
        {
            return _db.First<FormosPildymoLaikas>(
                @"select
                  floor(avg(a.valandos*60+a.minutes)/60) valandos
                  ,mod(round(avg(a.valandos*60+a.minutes)),60) minutes 
                  from
                  osfi.formospildymolaikas a
                  ,osfi.imone i
                  where 
                      a.dataiki is null
                  and i.id = a.imone_id
                  and i.asmenskodas in (" + String.Join(",", imoniuKodai) + @")
                  order by 1
                  ");
        }

        public FormosPildymoLaikas GetFormosPildymoLaikas(int metai)
        {
            throw new NotImplementedException();
        }
        */

    }
}