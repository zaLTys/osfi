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
        Rezis
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