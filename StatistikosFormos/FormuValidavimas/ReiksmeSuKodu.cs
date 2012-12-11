namespace StatistikosFormos.FormuValidavimas
{
    public class ReiksmeSuKodu
    {
        public string Kodas { get; private set; }
        public decimal? Reiksme { get; private set; }

        public ReiksmeSuKodu(string kodas, decimal? reiksme)
        {
            Kodas = kodas;
            Reiksme = reiksme;
        }
    }
}