namespace Vic.ZubStatistika.Entities
{
    public class KlaidosAprasas
    {
        public FormosTipas FormosTipas { get; set; }
        public int IrasoId { get; set; }
        public int Stulpelis { get; set; }
        public KlaidosKodas KlaidosKodas { get; set; }

        protected bool Equals(KlaidosAprasas other)
        {
            return FormosTipas.Equals(other.FormosTipas) && IrasoId == other.IrasoId && Stulpelis == other.Stulpelis;
        }

        public KlaidosAprasas()
        {
            
        }

        public KlaidosAprasas(FormosTipas formosTipas, int irasoId, int stulpelis, KlaidosKodas klaidosKodas)
        {
            FormosTipas = formosTipas;
            IrasoId = irasoId;
            Stulpelis = stulpelis;
            KlaidosKodas = klaidosKodas;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KlaidosAprasas) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = FormosTipas.GetHashCode();
                hashCode = (hashCode*397) ^ IrasoId;
                hashCode = (hashCode*397) ^ Stulpelis;
                return hashCode;
            }
        }
    }

    public enum KlaidosKodas
    {
        Neneigiamas,
        IsJu,
        NeNulis
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