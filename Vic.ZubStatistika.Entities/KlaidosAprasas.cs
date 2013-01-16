namespace Vic.ZubStatistika.Entities
{
    public class KlaidosAprasas
    {
        public virtual int Id { get; set; }
        public virtual Upload Upload { get; set; }
        public virtual FormosTipas FormosTipas { get; set; }
        public virtual string IrasoKodas { get; set; }
        public virtual int Stulpelis { get; set; }
        public virtual KlaidosKodas KlaidosKodas { get; set; }

        public KlaidosAprasas()
        {
            
        }

        public KlaidosAprasas(FormosTipas formosTipas, Upload upload, string irasoKodas, int stulpelis, KlaidosKodas klaidosKodas)
        {
            FormosTipas = formosTipas;
            Upload = upload;
            IrasoKodas = irasoKodas;
            Stulpelis = stulpelis;
            KlaidosKodas = klaidosKodas;
        }
    }

    public enum KlaidosKodas
    {
        Neneigiamas,
        IsJu,
        NeNulis,
        Rezis,
        DarboDienosTrukme,
        F01K01,
        F01K02,
        F01K03,
        F01K04,
        F01K05,
        F01K06,
        F01K07,
        F02K01,
        F02K02,
        F02K03,
        F02K04,
        F02K05,
        F03K01,
        F03K02,
        F03K03,
        F03K04,
        F03K05,
        F03K06,
        F03K07,
        F03K08,
        F03K09,
        F41K01,
        F41K02,
        F41K03,
        F41K04,
        F41K05,
        F41K06,
        F41K07,
        F42K01,
        F42K02,
        Fplk01,
        F05K01,
        F05K02,
        F06K01,
        F07K01,
        F08K01,
        F08K02,
        F08K03,
        F08K04,
        F08K05,
        F08K06,
        F09K01,
        F09K02,
        F09K03,
        F09K04,

        F07K02,
        F41K08
    }

    public enum FormosTipas
    {
        Augalininkyste,
        Darbuotojai,
        DotacijosSubsidijos,
        FormosPildymoLaikas,
        Gyvulininkyste,
        GyvuliuSkaicius,
        IlgalaikisTurtas,
        ProdukcijosKaita,
        ProduktuPardavimas,
        Sanaudos,
        ZemesPlotai
    }
}