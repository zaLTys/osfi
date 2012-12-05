using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.FormuValidavimas
{
    public interface IUploadValidator
    {
        IEnumerable<KlaidosAprasas> Validate(Upload upload);
    }

    public class UploadValidator : IUploadValidator
    {
        public IEnumerable<KlaidosAprasas> Validate(Upload upload)
        {
            return ValidateIlgalaikisTurtas(upload.IlgalaikisTurtas)
                .Concat(ValidateDarbuotojai(upload.Darbuotojai))
                .Concat(ValidateAugalininkyste(upload.Augalininkyste))
                .Concat(ValidateDotacijosSubsidijos(upload.DotacijosSubsidijos))
                .Concat(ValidateFormosPildymoLaikas(upload.FormosPildymoLaikas))
                .Concat(ValidateGyvulininkyste(upload.Gyvulininkyste))
                .Concat(ValidateGyvuliuSkaicius(upload.GyvuliuSkaicius))
                .Concat(ValidateProdukcijosKaita(upload.ProdukcijosKaita))
                .Concat(ValidateProduktuPardavimas(upload.ProduktuPardavimas))
                .Concat(ValidateSanaudos(upload.Sanaudos))
                .Concat(ValidateZemesPlotai(upload.ZemesPlotai));
            //.Union(...)
        }

        //--------------------------------------NEVERTIKALIAI--------------------------------------\\

        private IEnumerable<KlaidosAprasas> ValidateZemesPlotai(ICollection<ZemesPlotai> zemesPlotai)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateSanaudos(ICollection<Sanaudos> sanaudos)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateProduktuPardavimas(ICollection<ProduktuPardavimas> produktuPardavimas)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateProdukcijosKaita(ICollection<ProdukcijosKaita> produkcijosKaita)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvuliuSkaicius(ICollection<GyvuliuSkaicius> gyvuliuSkaicius)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvulininkyste(ICollection<Gyvulininkyste> gyvulininkyste)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateFormosPildymoLaikas(ICollection<FormosPildymoLaikas> formosPildymoLaikas)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<KlaidosAprasas> ValidateDotacijosSubsidijos(IEnumerable<DotacijosSubsidijos> irasai) //irasai?
        {
            var neneigiamosKlaidos = ValidateDotacijosSubsidijosVertikaliai(irasai, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0]));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateDotacijosSubsidijosVertikaliai(irasai, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011", "012", "013" }));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;


        }

        private IEnumerable<KlaidosAprasas> ValidateAugalininkyste(ICollection<Augalininkyste> irasai)
        {
             foreach (var augalininkyste in irasai)
             {
                 if (augalininkyste.IslaidosVisos < augalininkyste.IslaidosPagrindinei) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, ilgalaikisTurtas.Id, 3, KlaidosKodas.IsJu);
             }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Ar neneigiamas
        private IEnumerable<KlaidaSuId> IrasaiTuriButiNeneigiami(int stulpelis, IEnumerable<ReiksmeSuKodu> reiksmesSuKodais, int[] ignoruojamiStulpeliai)
        {
            if (!ignoruojamiStulpeliai.Contains(stulpelis))
            {
                foreach (var reiksmeSuKodu in reiksmesSuKodais)
                {
                    if (reiksmeSuKodu.Reiksme.HasValue && reiksmeSuKodu.Reiksme < 0) yield return new KlaidaSuId(reiksmeSuKodu.Id, KlaidosKodas.Neneigiamas);
                }
            }
        }
        //EiluciuSuma
        private IEnumerable<KlaidaSuId> EiluciuSumaTuriButiNemazesne(int stulpelis, IEnumerable<ReiksmeSuKodu> reiksmesSuKodais, string sumosKodas, string[] daliuKodai)
        {
            var suma = reiksmesSuKodais.First(x => x.Kodas == sumosKodas);
            var dalys = reiksmesSuKodais.Where(x => x.Reiksme.HasValue && daliuKodai.Contains(x.Kodas));

            if (dalys.Sum(x => x.Reiksme.Value) > suma.Reiksme) yield return new KlaidaSuId(suma.Id, KlaidosKodas.IsJu);
        }
        //Ar ne nulis ---patikrinti
        private IEnumerable<KlaidaSuId> EiluteNeNulis(int stulpelis, IEnumerable<ReiksmeSuKodu> reiksmesSuKodais, string eiluteskodas)
        {
            var eilute = reiksmesSuKodais.First(x => x.Kodas == eiluteskodas);
            if (eilute.Reiksme == 0) yield return new KlaidaSuId(eilute.Id, KlaidosKodas.NeNulis);
        }


        private IEnumerable<KlaidosAprasas> ValidateIlgalaikisTurtas(IEnumerable<IlgalaikisTurtas> irasai) //padaryta-----------
        {
            var neneigiamosKlaidos = ValidateIlgalaikisTurtasVertikaliai(irasai, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new[]{4,12}));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateIlgalaikisTurtasVertikaliai(irasai, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "020", new[] { "021", "022", "023", "024", "025", "026", "027", "028" }));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            blogosSumos = ValidateIlgalaikisTurtasVertikaliai(irasai, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011", "012" }));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            blogosSumos = ValidateIlgalaikisTurtasVertikaliai(irasai, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "030", new[] { "031", "032" }));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            foreach (var ilgalaikisTurtas in irasai)
            {
                if (ilgalaikisTurtas.Gauta < ilgalaikisTurtas.IsJuNauju) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, ilgalaikisTurtas.Id, 3, KlaidosKodas.IsJu);
                if (ilgalaikisTurtas.NurasytaIlgalaikio < (ilgalaikisTurtas.LikviduotaIlgalaikio + ilgalaikisTurtas.ParduotaIlgalaikio)) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, ilgalaikisTurtas.Id, 5, KlaidosKodas.IsJu);
                if (ilgalaikisTurtas.NurasytaNusidevejimo < ilgalaikisTurtas.LikviduotaNusidevejimo) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, ilgalaikisTurtas.Id, 13, KlaidosKodas.IsJu);   
            }
        }

        
        private IEnumerable<KlaidosAprasas> ValidateDarbuotojai(IEnumerable<Darbuotojai> irasai)
        {
            var neneigiamosKlaidos = ValidateDarbuotojaiVertikaliai(irasai, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0]));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateDarbuotojaiVertikaliai(irasai, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "040", new[] { "041", "042", "043", "044" }));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            var neNuliai = ValidateDarbuotojaiVertikaliai(irasai, (stulpelis, reiksmesSuKodais) => EiluteNeNulis(stulpelis, reiksmesSuKodais, "010");
            foreach (var blogaSuma in neNuliai) yield return blogaSuma; //Ar cia gerai?

        }
       
        //|||||||||||||||||||||||||||||||||||||||||||||||||||||VERTIKALIAI|||||||||||||||||||||||||||||||||||||||||||||\\

        private IEnumerable<KlaidosAprasas> ValidateIlgalaikisTurtasVertikaliai(IEnumerable<IlgalaikisTurtas> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test( 1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.LikutisPradziojeIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id,  1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Gauta)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IsJuNauju)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.VertesPadidejimas)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.NurasytaIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.LikviduotaIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ParduotaIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Nukainota)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.LikutisPabaigojeIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.LikutisPradziojeNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 10, klaida.Kodas);
            foreach (var klaida in test(11, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Priskaiciuota)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 11, klaida.Kodas);
            foreach (var klaida in test(12, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Pasikeitimas)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 12, klaida.Kodas);
            foreach (var klaida in test(13, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.NurasytaNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 13, klaida.Kodas);
            foreach (var klaida in test(14, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.LikviduotaNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 14, klaida.Kodas);
            foreach (var klaida in test(15, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.LikutisPabaigojeNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, klaida.Id, 15, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateDarbuotojaiVertikaliai(IEnumerable<Darbuotojai> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Reiksme)))) yield return new KlaidosAprasas(FormosTipas.Darbuotojai, klaida.Id, 1, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateDotacijosSubsidijosVertikaliai(IEnumerable<DotacijosSubsidijos> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Suma)))) yield return new KlaidosAprasas(FormosTipas.DotacijosSubsidijos, klaida.Id, 1, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateSanaudosVertikaliai(IEnumerable<Sanaudos> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IsViso)))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, klaida.Id, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Augalininkyste)))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, klaida.Id, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Gyvulininkyste)))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, klaida.Id, 3, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateProduktuPardavimasVertikaliai(IEnumerable<ProduktuPardavimas> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ParduotaNatura)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, klaida.Id, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ParduotaEksportui)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, klaida.Id, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ParduotaIskaitomuojuSvoriu)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, klaida.Id, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ProdukcijosSavikaina)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, klaida.Id, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.PardavimuPajamos)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, klaida.Id, 5, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateAugalininkysteVertikaliai(IEnumerable<Augalininkyste> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Plotas)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ProdukcijosKiekis)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Derlingumas)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosDarboApmokejimas)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosSeklos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosTrasos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosNafta)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosElektra)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosKitos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosVisos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 10, klaida.Kodas);
            foreach (var klaida in test(11, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosPagrindinei)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 11, klaida.Kodas);
            foreach (var klaida in test(12, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ProdukcijosSavikaina)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, klaida.Id, 12, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvulininkysteVertikaliai(IEnumerable<Gyvulininkyste> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.VidutinisGyvuliuSk)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ProdukcijosKiekis)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosDarboApmokejimas)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosPasarai)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosNafta)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosElektra)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosKitos)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosVisos)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosPagrindinei)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.ProdukcijosSavikaina)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, klaida.Id, 10, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateProdukcijosKaitaVertikaliai(IEnumerable<ProdukcijosKaita> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.MetuPradziosLikutis)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.PajamosPagaminta)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.PajamosPirkta)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.PajamosImportuota)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosVisos)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosParduota\)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosPasarui)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosSeklai)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosDuotaPerdirbti)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosProdukcijosNuostoliai)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 10, klaida.Kodas);
            foreach (var klaida in test(11, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.IslaidosKitos)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 11, klaida.Kodas);
            foreach (var klaida in test(12, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.MetuPabaigosLikutis)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, klaida.Id, 12, klaida.Kodas);
        }

         private IEnumerable<KlaidosAprasas> ValidateGyvuliuSkaiciusVertikaliai(IEnumerable<GyvuliuSkaicius> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
         {
             foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.MetuPradzioje)))) yield return new KlaidosAprasas(FormosTipas.GyvuliuSkaicius, klaida.Id, 1, klaida.Kodas);
             foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.MetuPabaigojeVnt)))) yield return new KlaidosAprasas(FormosTipas.GyvuliuSkaicius, klaida.Id, 2, klaida.Kodas);
             foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.MetuPabaigojeVerte)))) yield return new KlaidosAprasas(FormosTipas.GyvuliuSkaicius, klaida.Id, 3, klaida.Kodas);
         }

        private IEnumerable<KlaidosAprasas> ValidateZemesPlotaiVertikaliai(IEnumerable<ZemesPlotai> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
         {
             foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.NuomaIsValstybes)))) yield return new KlaidosAprasas(FormosTipas.ZemesPlotai, klaida.Id, 1, klaida.Kodas);
             foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.NuomaIsFiziniu)))) yield return new KlaidosAprasas(FormosTipas.ZemesPlotai, klaida.Id, 2, klaida.Kodas);
             foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.NuosavaZeme)))) yield return new KlaidosAprasas(FormosTipas.ZemesPlotai, klaida.Id, 3, klaida.Kodas);
         }

    /*    private IEnumerable<KlaidosAprasas> ValidateFormosPildymoLaikasVertikaliai(IEnumerable<FormosPildymoLaikas> irasai, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuId>> test)
        {
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Valandos)))) yield return new KlaidosAprasas(FormosTipas.FormosPildymoLaikas, klaida.Id, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Id, x.Rusis.Kodas, x.Minutes)))) yield return new KlaidosAprasas(FormosTipas.FormosPildymoLaikas, klaida.Id, 2, klaida.Kodas);
        }*/ //??????????????????????????????????????????????????????????????????????????????????????????????????????????



        
    }

    public class ReiksmeSuKodu
    {
        public int Id { get; private set; }
        public string Kodas { get; private set; }
        public decimal? Reiksme { get; private set; }

        public ReiksmeSuKodu(int id, string kodas, decimal? reiksme)
        {
            Id = id;
            Kodas = kodas;
            Reiksme = reiksme;
        }
    }

    public class KlaidaSuId
    {
        public int Id { get; private set; }
        public KlaidosKodas Kodas { get; private set; }

        public KlaidaSuId(int id, KlaidosKodas kodas)
        {
            Id = id;
            Kodas = kodas;
        }
    }
}
