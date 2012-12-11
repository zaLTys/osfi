using Vic.ZubStatistika.Entities;

namespace StatistikosFormos.FormuValidavimas
{
    public class KlaidaSuRusiesKodu
    {
        public string RusiesKodas { get; private set; }
        public KlaidosKodas Kodas { get; private set; }

        public KlaidaSuRusiesKodu(string rusiesKodas, KlaidosKodas kodas)
        {
            RusiesKodas = rusiesKodas;
            Kodas = kodas;
        }
    }
}