using System;
using System.Collections.Generic;
using System.Linq;
using Vic.ZubStatistika.Entities;

namespace StatistikosFormos.FormuValidavimas
{
    public class UploadValidator : IUploadValidator
    {
        public IEnumerable<KlaidosAprasas> Validate(Upload upload)
        {
            return ValidateIlgalaikisTurtas(upload)
                .Concat(ValidateDarbuotojai(upload))
                .Concat(ValidateAugalininkyste(upload))
                .Concat(ValidateDotacijosSubsidijos(upload))
                .Concat(ValidateFormosPildymoLaikas(upload))
                .Concat(ValidateGyvulininkyste(upload))
                .Concat(ValidateGyvuliuSkaicius(upload))
                .Concat(ValidateProdukcijosKaita(upload))
                .Concat(ValidateProduktuPardavimas(upload))
                .Concat(ValidateSanaudos(upload))
                .Concat(ValidateZemesPlotai(upload));
            //.Union(...)
        }

        //--------------------------------------NEVERTIKALIAI--------------------------------------\\

        private IEnumerable<KlaidosAprasas> ValidateZemesPlotai(Upload upload)
        {
            var neneigiamosKlaidos = ValidateZemesPlotaiVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F09K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateZemesPlotaiVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "090", "100" }, KlaidosKodas.F09K02));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateZemesPlotaiVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "020", new[] { "030", "040", "060" }, KlaidosKodas.F09K03));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateZemesPlotaiVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "040", new[] { "050" }, KlaidosKodas.F09K04));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
        }

        private IEnumerable<KlaidosAprasas> ValidateSanaudos(Upload upload)
        {
            var irasai = upload.Sanaudos;
            var neneigiamosKlaidos = ValidateProdukcijosKaitaVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F03K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;


            foreach (var sanaudos in upload.Sanaudos)
            {
                if (sanaudos.IsViso != ((sanaudos.Augalininkyste ?? 0) + (sanaudos.Gyvulininkyste ?? 0))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, upload, sanaudos.Rusis.Kodas, 1, KlaidosKodas.F03K02);
            }

            var blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011" }, KlaidosKodas.F03K03));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "020", new[] { "021", "022", "023", "024", "025", "026", "027", "028", "030", "031", "032" }, KlaidosKodas.F03K04));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "021", new[] { "021.1" }, KlaidosKodas.F03K05));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "022", new[] { "022.1" }, KlaidosKodas.F03K06));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "024", new[] { "024.1" }, KlaidosKodas.F03K07));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "050", new[] { "052", "053", "054", "055", "056", "057", "058" }, KlaidosKodas.F03K08));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateSanaudosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "057", new[] { "057.1", "057.2", "057.3" }, KlaidosKodas.F03K09));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
        }

        private IEnumerable<KlaidosAprasas> ValidateProduktuPardavimas(Upload upload)
        {
            var neneigiamosKlaidos = ValidateProduktuPardavimasVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F41K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;
            foreach (var produktuPardavimas in upload.ProduktuPardavimas)
            {
                if (produktuPardavimas.ParduotaNatura < produktuPardavimas.ParduotaEksportui) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, produktuPardavimas.Rusis.Kodas, 1, KlaidosKodas.F41K02);


                var isskyrus = new[] { "110", "330", "340", "360" };
                if (!isskyrus.Contains(produktuPardavimas.Rusis.Kodas))
                {
                    if (produktuPardavimas.ParduotaNatura < produktuPardavimas.ParduotaIskaitomuojuSvoriu) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, produktuPardavimas.Rusis.Kodas, 1, KlaidosKodas.F41K03);
                }
                else if (produktuPardavimas.Rusis.Kodas == "110")
                {
                    if (produktuPardavimas.ParduotaNatura != produktuPardavimas.ParduotaIskaitomuojuSvoriu) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, produktuPardavimas.Rusis.Kodas, 1, KlaidosKodas.F41K04);
                }
                else if (produktuPardavimas.Rusis.Kodas == "330")
                {
                    if (produktuPardavimas.ParduotaNatura != produktuPardavimas.ParduotaIskaitomuojuSvoriu) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, produktuPardavimas.Rusis.Kodas, 1, KlaidosKodas.F41K05);
                }
                else if (produktuPardavimas.Rusis.Kodas == "340")
                {
                    if (produktuPardavimas.ParduotaNatura <= produktuPardavimas.ParduotaIskaitomuojuSvoriu) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, produktuPardavimas.Rusis.Kodas, 1, KlaidosKodas.F41K06);
                }
                else
                {
                    if (produktuPardavimas.ParduotaNatura != produktuPardavimas.ParduotaIskaitomuojuSvoriu) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, produktuPardavimas.Rusis.Kodas, 1, KlaidosKodas.F41K07);
                }
            }

            var blogosSumos = ValidateProduktuPardavimasVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "200", new[] { "210", "220", "230", "240", "250", "260", "270", "280", "290", "300", "310", "320" }, KlaidosKodas.F41K08));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;


        }

        private IEnumerable<KlaidosAprasas> ValidateProdukcijosKaita(Upload upload)
        {
            var irasai = upload.ProdukcijosKaita;
            var neneigiamosKlaidos = ValidateProdukcijosKaitaVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F07K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateProdukcijosKaitaVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011", "012", "013", "014", "015", "016", "017", "018", "019", "020", "021", "022" }, KlaidosKodas.F07K02));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvuliuSkaicius(Upload upload)
        {
            var irasai = upload.GyvuliuSkaicius;
            var neneigiamosKlaidos = ValidateGyvuliuSkaiciusVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F08K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateGyvuliuSkaiciusVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011", "012", "013" }, KlaidosKodas.F08K02));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateGyvuliuSkaiciusVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "020", new[] { "021" }, KlaidosKodas.F08K03));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateGyvuliuSkaiciusVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "030", new[] { "031" }, KlaidosKodas.F08K04));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateGyvuliuSkaiciusVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "040", new[] { "041" }, KlaidosKodas.F08K05));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
            blogosSumos = ValidateGyvuliuSkaiciusVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "050", new[] { "051" }, KlaidosKodas.F08K06));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvulininkyste(Upload upload)
        {
            var neneigiamosKlaidos = ValidateGyvulininkysteVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F06K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

        }

        private IEnumerable<KlaidosAprasas> ValidateFormosPildymoLaikas(Upload upload)
        {
            var irasas = upload.FormosPildymoLaikas.First();
            if (irasas.Minutes < 0 || irasas.Minutes > 59) yield return new KlaidosAprasas(FormosTipas.FormosPildymoLaikas, upload, null, 2, KlaidosKodas.Fplk01);
            if (irasas.Valandos < 0) yield return new KlaidosAprasas(FormosTipas.FormosPildymoLaikas, upload, null, 1, KlaidosKodas.Fplk01);
            if (irasas.Valandos + irasas.Minutes == 0) yield return new KlaidosAprasas(FormosTipas.FormosPildymoLaikas, upload, null, 1, KlaidosKodas.Fplk01);
        }

        private IEnumerable<KlaidosAprasas> ValidateDotacijosSubsidijos(Upload upload) //irasai?
        {
            var irasai = upload.DotacijosSubsidijos;

            var neneigiamosKlaidos = ValidateDotacijosSubsidijosVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F42K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateDotacijosSubsidijosVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011", "012", "013" }, KlaidosKodas.F42K02));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;


        }

        private IEnumerable<KlaidosAprasas> ValidateAugalininkyste(Upload upload)
        {
            var neneigiamosKlaidos = ValidateAugalininkysteVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F05K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            foreach (var augalininkyste in upload.Augalininkyste)
            {
                if (augalininkyste.IslaidosVisos < augalininkyste.IslaidosPagrindinei) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, augalininkyste.Rusis.Kodas, 3, KlaidosKodas.F05K02);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Ar neneigiamas
        private IEnumerable<KlaidaSuRusiesKodu> IrasaiTuriButiNeneigiami(int stulpelis, IEnumerable<ReiksmeSuKodu> reiksmesSuKodais, int[] ignoruojamiStulpeliai, KlaidosKodas klaidosKodas)
        {
            if (!ignoruojamiStulpeliai.Contains(stulpelis))
            {
                foreach (var reiksmeSuKodu in reiksmesSuKodais)
                {
                    if (reiksmeSuKodu.Reiksme.HasValue && reiksmeSuKodu.Reiksme < 0) yield return new KlaidaSuRusiesKodu(reiksmeSuKodu.Kodas, klaidosKodas);
                }
            }
        }
        //EiluciuSuma
        private IEnumerable<KlaidaSuRusiesKodu> EiluciuSumaTuriButiNemazesne(int stulpelis, IEnumerable<ReiksmeSuKodu> reiksmesSuKodais, string sumosKodas, string[] daliuKodai, KlaidosKodas klaidosKodas)
        {
            var suma = reiksmesSuKodais.First(x => x.Kodas == sumosKodas);
            var dalys = reiksmesSuKodais.Where(x => x.Reiksme.HasValue && daliuKodai.Contains(x.Kodas));

            if (dalys.Sum(x => x.Reiksme.Value) > suma.Reiksme) yield return new KlaidaSuRusiesKodu(suma.Kodas, klaidosKodas);
        }
        //Ar ne nulis ---patikrinti
        private IEnumerable<KlaidaSuRusiesKodu> EiluteNeNulis(int stulpelis, IEnumerable<ReiksmeSuKodu> reiksmesSuKodais, string eiluteskodas, KlaidosKodas klaidosKodas)
        {
            var eilute = reiksmesSuKodais.First(x => x.Kodas == eiluteskodas);
            if (eilute.Reiksme == 0) yield return new KlaidaSuRusiesKodu(eilute.Kodas, klaidosKodas);
        }


        private IEnumerable<KlaidosAprasas> ValidateIlgalaikisTurtas(Upload upload) //padaryta-----------
        {
            var irasai = upload.IlgalaikisTurtas;

            var neneigiamosKlaidos = ValidateIlgalaikisTurtasVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new[] { 4, 12 }, KlaidosKodas.F01K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateIlgalaikisTurtasVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "020", new[] { "021", "022", "023", "024", "025", "026", "027", "028" }, KlaidosKodas.F01K06));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            blogosSumos = ValidateIlgalaikisTurtasVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "010", new[] { "011", "012" }, KlaidosKodas.F01K05));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            blogosSumos = ValidateIlgalaikisTurtasVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "030", new[] { "031", "032" }, KlaidosKodas.F01K07));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            foreach (var ilgalaikisTurtas in irasai)
            {
                if (ilgalaikisTurtas.Gauta < ilgalaikisTurtas.IsJuNauju) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, ilgalaikisTurtas.Rusis.Kodas, 2, KlaidosKodas.F01K02);
                if (ilgalaikisTurtas.NurasytaIlgalaikio < (ilgalaikisTurtas.LikviduotaIlgalaikio + ilgalaikisTurtas.ParduotaIlgalaikio)) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, ilgalaikisTurtas.Rusis.Kodas, 5, KlaidosKodas.F01K03);
                if (ilgalaikisTurtas.NurasytaNusidevejimo < ilgalaikisTurtas.LikviduotaNusidevejimo) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, ilgalaikisTurtas.Rusis.Kodas, 13, KlaidosKodas.F01K04);
            }
        }


        private IEnumerable<KlaidosAprasas> ValidateDarbuotojai(Upload upload)
        {
            var irasai = upload.Darbuotojai;

            var neneigiamosKlaidos = ValidateDarbuotojaiVertikaliai(upload, (stulpelis, reiksmeSuKodais) => IrasaiTuriButiNeneigiami(stulpelis, reiksmeSuKodais, new int[0], KlaidosKodas.F02K01));
            foreach (var neneigiamaKlaida in neneigiamosKlaidos) yield return neneigiamaKlaida;

            var blogosSumos = ValidateDarbuotojaiVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluciuSumaTuriButiNemazesne(stulpelis, reiksmesSuKodais, "040", new[] { "041", "042", "043", "044" }, KlaidosKodas.F02K03));
            foreach (var blogaSuma in blogosSumos) yield return blogaSuma;

            var neNuliai = ValidateDarbuotojaiVertikaliai(upload, (stulpelis, reiksmesSuKodais) => EiluteNeNulis(stulpelis, reiksmesSuKodais, "010", KlaidosKodas.F02K04));
            foreach (var blogaSuma in neNuliai) yield return blogaSuma;

            var dirbtaDienu = upload.Darbuotojai.First(x => x.Rusis.Kodas == "020").Reiksme;
            var dirbtaValandu = upload.Darbuotojai.First(x => x.Rusis.Kodas == "030").Reiksme;
            if (dirbtaDienu > dirbtaValandu) yield return new KlaidosAprasas(FormosTipas.Darbuotojai, upload, "020", 1, KlaidosKodas.F02K02);
            if (dirbtaValandu / dirbtaDienu > 11) yield return new KlaidosAprasas(FormosTipas.Darbuotojai, upload, "020", 1, KlaidosKodas.F02K05);

        }

        //|||||||||||||||||||||||||||||||||||||||||||||||||||||VERTIKALIAI|||||||||||||||||||||||||||||||||||||||||||||\\
        private IEnumerable<KlaidosAprasas> ValidateIlgalaikisTurtasVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.IlgalaikisTurtas;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.LikutisPradziojeIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Gauta)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IsJuNauju)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.VertesPadidejimas)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.NurasytaIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.LikviduotaIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ParduotaIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Nukainota)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.LikutisPabaigojeIlgalaikio)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.LikutisPradziojeNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 10, klaida.Kodas);
            foreach (var klaida in test(11, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Priskaiciuota)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 11, klaida.Kodas);
            foreach (var klaida in test(12, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Pasikeitimas)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 12, klaida.Kodas);
            foreach (var klaida in test(13, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.NurasytaNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 13, klaida.Kodas);
            foreach (var klaida in test(14, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.LikviduotaNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 14, klaida.Kodas);
            foreach (var klaida in test(15, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.LikutisPabaigojeNusidevejimo)))) yield return new KlaidosAprasas(FormosTipas.IlgalaikisTurtas, upload, klaida.RusiesKodas, 15, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateDarbuotojaiVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.Darbuotojai;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Reiksme)))) yield return new KlaidosAprasas(FormosTipas.Darbuotojai, upload, klaida.RusiesKodas, 1, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateDotacijosSubsidijosVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.DotacijosSubsidijos;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Suma)))) yield return new KlaidosAprasas(FormosTipas.DotacijosSubsidijos, upload, klaida.RusiesKodas, 1, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateSanaudosVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.Sanaudos;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IsViso)))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Augalininkyste)))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Gyvulininkyste)))) yield return new KlaidosAprasas(FormosTipas.Sanaudos, upload, klaida.RusiesKodas, 3, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateProduktuPardavimasVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.ProduktuPardavimas;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ParduotaNatura)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ParduotaEksportui)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ParduotaIskaitomuojuSvoriu)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, klaida.RusiesKodas, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ProdukcijosSavikaina)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, klaida.RusiesKodas, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.PardavimuPajamos)))) yield return new KlaidosAprasas(FormosTipas.ProduktuPardavimas, upload, klaida.RusiesKodas, 5, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateAugalininkysteVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.Augalininkyste;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Plotas)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ProdukcijosKiekis)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.Derlingumas)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosDarboApmokejimas)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosSeklos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosTrasos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosNafta)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosElektra)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosKitos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosVisos)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 10, klaida.Kodas);
            foreach (var klaida in test(11, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosPagrindinei)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 11, klaida.Kodas);
            foreach (var klaida in test(12, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ProdukcijosSavikaina)))) yield return new KlaidosAprasas(FormosTipas.Augalininkyste, upload, klaida.RusiesKodas, 12, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvulininkysteVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.Gyvulininkyste;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.VidutinisGyvuliuSk)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ProdukcijosKiekis)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosDarboApmokejimas)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosPasarai)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosNafta)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosElektra)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosKitos)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosVisos)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosPagrindinei)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.ProdukcijosSavikaina)))) yield return new KlaidosAprasas(FormosTipas.Gyvulininkyste, upload, klaida.RusiesKodas, 10, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateProdukcijosKaitaVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.ProdukcijosKaita;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.MetuPradziosLikutis)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.PajamosPagaminta)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.PajamosPirkta)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 3, klaida.Kodas);
            foreach (var klaida in test(4, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.PajamosImportuota)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 4, klaida.Kodas);
            foreach (var klaida in test(5, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosVisos)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 5, klaida.Kodas);
            foreach (var klaida in test(6, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosParduota)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 6, klaida.Kodas);
            foreach (var klaida in test(7, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosPasarui)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 7, klaida.Kodas);
            foreach (var klaida in test(8, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosSeklai)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 8, klaida.Kodas);
            foreach (var klaida in test(9, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosDuotaPerdirbti)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 9, klaida.Kodas);
            foreach (var klaida in test(10, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosProdukcijosNuostoliai)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 10, klaida.Kodas);
            foreach (var klaida in test(11, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.IslaidosKitos)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 11, klaida.Kodas);
            foreach (var klaida in test(12, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.MetuPabaigosLikutis)))) yield return new KlaidosAprasas(FormosTipas.ProdukcijosKaita, upload, klaida.RusiesKodas, 12, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateGyvuliuSkaiciusVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.GyvuliuSkaicius;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.MetuPradzioje)))) yield return new KlaidosAprasas(FormosTipas.GyvuliuSkaicius, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.MetuPabaigojeVnt)))) yield return new KlaidosAprasas(FormosTipas.GyvuliuSkaicius, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.MetuPabaigojeVerte)))) yield return new KlaidosAprasas(FormosTipas.GyvuliuSkaicius, upload, klaida.RusiesKodas, 3, klaida.Kodas);
        }

        private IEnumerable<KlaidosAprasas> ValidateZemesPlotaiVertikaliai(Upload upload, Func<int, IEnumerable<ReiksmeSuKodu>, IEnumerable<KlaidaSuRusiesKodu>> test)
        {
            var irasai = upload.ZemesPlotai;
            foreach (var klaida in test(1, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.NuomaIsValstybes)))) yield return new KlaidosAprasas(FormosTipas.ZemesPlotai, upload, klaida.RusiesKodas, 1, klaida.Kodas);
            foreach (var klaida in test(2, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.NuomaIsFiziniu)))) yield return new KlaidosAprasas(FormosTipas.ZemesPlotai, upload, klaida.RusiesKodas, 2, klaida.Kodas);
            foreach (var klaida in test(3, irasai.Select(x => new ReiksmeSuKodu(x.Rusis.Kodas, x.NuosavaZeme)))) yield return new KlaidosAprasas(FormosTipas.ZemesPlotai, upload, klaida.RusiesKodas, 3, klaida.Kodas);
        }

    }
}