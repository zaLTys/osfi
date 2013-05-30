using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate.Tool.hbm2ddl;
using Vic.ZubStatistika.DataAccess;
using Vic.ZubStatistika.DataAccess.Mappings;
using Vic.ZubStatistika.Entities;
using FluentNHibernate.Conventions;

namespace Vic.ZubStatistika.DatabaseCreate
{
    public class SchemaConvention : IClassConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance)
        {
            instance.Schema("OSFI");
        }
    }
    class Program
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            var exporter = new SchemaExport(config);
            exporter.SetOutputFile("output.sql");
            exporter.Create(true, false);
        }

        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();

            try
            {
                var connString = ConfigurationManager.ConnectionStrings["FormosDuombaze"].ConnectionString;

                var configuration = Fluently.Configure()
.Database(OracleDataClientConfiguration.Oracle10
                                                                              .ConnectionString(connString).
                                                                              DefaultSchema("OSFI"))
                                                                .Mappings(m => m.AutoMappings.Add(
                                                                    AutoMap.AssemblyOf<Augalininkyste>()
                                                                        .Conventions.Add(
                                                                            new NHibernateSequenceConvention(),
                                                                            new CascadeAllConvention(),
                                                                            Table.Is(x => x.TableName.ToUpper()))
                                                                            .UseOverridesFromAssemblyOf<UploadMapOverride>))
                    .ExposeConfiguration(BuildSchema)
                    .BuildConfiguration();
                    //.BuildSessionFactory();

                Log.Info("Initializing database with data");

                //using (var session = sessionFactory.OpenSession())
                //using (var transaction = session.BeginTransaction())
                //{
                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Nematerialusis turtas( 011, 012 kodai)"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "kompiuterinė programinė įranga"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "012",
                //        Pavadinimas = "kitas nematerialusis turtas"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Materialusis turtas( 021, 022, 023, 024, 025, 026, 027, 028 kodai)"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "021",
                //        Pavadinimas = "Žemė"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "022",
                //        Pavadinimas = "Miškai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "023",
                //        Pavadinimas = "Pastatai ir kiti statiniai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "024",
                //        Pavadinimas = "Mašinos ir įrenginiai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "025",
                //        Pavadinimas = "Transporto priemonės"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "026",
                //        Pavadinimas = "Kita įranga, prietaisai, įrankiai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "027",
                //        Pavadinimas = "Kitas materialusis turtas"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "028",
                //        Pavadinimas = "Statyba ir kiti kapitaliniai darbai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Ilgalaikis finansinis turtas( 031, 032 kodai)"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "031",
                //        Pavadinimas = "Nuosavybės vertybiniai popieriai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "032",
                //        Pavadinimas = "Skolos vertybiniai popieriai"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Biologinis turtas( daugiamečiai sodiniai)"
                //    });

                //    session.Save(new IlgalaikioTurtoRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Iš viso( 010, 020, 030, 040 kodai)"
                //    });

                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Vidutinis metinis įmonėje dirbančiųjų skaičius"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Dirbta dienų (tūkst.)*"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Dirbta valandų (tūkst.)"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Apskaičiuota darbo apmokėjimo suma visiems darbuotojams, tūkst. Lt"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "041",
                //        Pavadinimas = "iš jų: apmokėjimas natūra"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "042",
                //        Pavadinimas = "iš jų: delspinigiai"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "043",
                //        Pavadinimas = "iš jų: išeitinės pašalpos, kompensacijos"
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "044",
                //        Pavadinimas = "iš jų: premijos "
                //    });
                //    session.Save(new DarbuotojuRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Yra pajininkų (akcininkų) metų pabaigoje"
                //    });

                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Darbo apmokėjimas su atskaitymais "
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "iš jo: valstybiniam socialiniam ir sveikatos draudimui"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Materialinės išlaidos, iš viso"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "021",
                //        Pavadinimas = "iš jų: sėklos ir sodinamoji medžiaga"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "021.1",
                //        Pavadinimas = "iš jų: pirktos sėklos ir sodinamoji medžiaga"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "022",
                //        Pavadinimas = "pašarai "
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "022.1",
                //        Pavadinimas = "iš jų: kombinuotieji pašarai"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "023",
                //        Pavadinimas = "trąšos ir dirvos pagerinimo medžiagos"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "024",
                //        Pavadinimas = "naftos produktai ir dujos"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "024.1",
                //        Pavadinimas = "iš jų: dyzelinas"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "025",
                //        Pavadinimas = "elektros energija"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "026",
                //        Pavadinimas = "augalų apsaugos priemonės"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "027",
                //        Pavadinimas = "farmaciniai preparatai"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "028",
                //        Pavadinimas = "atsarginės dalys"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "ž. ū. pastatų remontas"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "031",
                //        Pavadinimas = "veterinarijos paslaugų apmokėjimas"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "032",
                //        Pavadinimas = "kitos materialinės išlaidos"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Ilgalaikio turto nusidėvėjimas"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Kitos išlaidos, iš viso"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "052",
                //        Pavadinimas = "sumokėtos palūkanos už paskolas"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "053",
                //        Pavadinimas = "žemės nuomos mokestis"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "054",
                //        Pavadinimas = "žemės mokestis"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "055",
                //        Pavadinimas = "kito ilgalaikio turto nuomos išlaidos"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "056",
                //        Pavadinimas = "draudimo išlaidos"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "057",
                //        Pavadinimas = "kiti mokesčiai, susiję su gamyba"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "057.1",
                //        Pavadinimas = "iš jų: pirmas"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "057.2",
                //        Pavadinimas = "iš jų: antras"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "057.3",
                //        Pavadinimas = "iš jų: trečias"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "058",
                //        Pavadinimas = "kitos išlaidos"
                //    });
                //    session.Save(new SanauduRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "Iš viso: (kodai: 010, 020, 040, 050)"
                //    });

                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Grūdai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Linų sėmenys"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Linų šiaudeliai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Žieminiai rapsai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "Vasariniai rapsai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "070",
                //        Pavadinimas = "Bulvės"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "080",
                //        Pavadinimas = "Cukriniai runkeliai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "090",
                //        Pavadinimas = "Daugiamečių žolių sėkla"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "100",
                //        Pavadinimas = "Atviro grunto daržovės"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "110",
                //        Pavadinimas = "Uždaro grunto daržovės"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "120",
                //        Pavadinimas = "Vaisiai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "130",
                //        Pavadinimas = "Uogos"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "140",
                //        Pavadinimas = "Lauko gėlininkystė"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "150",
                //        Pavadinimas = "Šiltnamių gėlininkystė"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "160",
                //        Pavadinimas = "Pievagrybiai"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "170",
                //        Pavadinimas = "Kita augalininkystės produkcija"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "180",
                //        Pavadinimas = "Iš viso (nuo 010 iki 170 kodų suma)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "200",
                //        Pavadinimas = "Gyvuliai ir paukščiai gyvuoju svoriu (nuo 210 iki 320 kodų suma)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "210",
                //        Pavadinimas = "iš jų:   parduota mėsai (gyvuoju svoriu): galvijų"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "220",
                //        Pavadinimas = "kiaulių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "230",
                //        Pavadinimas = "avių ir ožkų"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "240",
                //        Pavadinimas = "paukščių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "250",
                //        Pavadinimas = "arklių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "260",
                //        Pavadinimas = "kitų gyvulių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "270",
                //        Pavadinimas = "parduota veislei (gyvuoju svoriu): galvijų"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "280",
                //        Pavadinimas = "kiaulių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "290",
                //        Pavadinimas = "kitų gyvulių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "300",
                //        Pavadinimas = "kitas pardavimas (gyvuoju svoriu):       galvijų"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "310",
                //        Pavadinimas = "kiaulių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "320",
                //        Pavadinimas = "kitų gyvulių"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "330",
                //        Pavadinimas = "Paukščiai (vienadieniai), tūkst. vnt."
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "340",
                //        Pavadinimas = "Pienas"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "350",
                //        Pavadinimas = "Vilna"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "360",
                //        Pavadinimas = "Kiaušiniai, tūkst. vnt."
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "380",
                //        Pavadinimas = "Kita gyvulininkystės produkcija"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "390",
                //        Pavadinimas = "Iš viso (200, 330, 340, 350, 360, 380 kodų suma)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "400",
                //        Pavadinimas = "Pramoninė produkcija"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "410",
                //        Pavadinimas = "iš jos:    žemės ūkio produkcijos apdorojimas ir perdirbimas"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "411",
                //        Pavadinimas = "medienos ir medienos gaminių gamyba (EVRK 20)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "412",
                //        Pavadinimas = "žemės ir miško ūkio traktorių (EVRK 29.31.20) ir kitos technikos remontas (EVRK 29.32, 10, 20, 30, 40, 50)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "413",
                //        Pavadinimas = "statyba (EVRK 45)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "500",
                //        Pavadinimas = "Kitos produkcijos, darbų ir paslaugų realizavimas"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "511",
                //        Pavadinimas = "iš jo:   didmeninė prekyba (EVRK 51 išskyrus 51.1 didmeninė    prekyba už atlyginimą ar pagal sutartį)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "512",
                //        Pavadinimas = "mažmeninė prekyba (EVRK 52 išskyrus 52.7 asmeninių ir namų ūkio reikmenų taisymas)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "513",
                //        Pavadinimas = "žemės ūkio paslaugos (EVRK 01.4)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "514",
                //        Pavadinimas = "transportas (EVRK 60.24)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "515",
                //        Pavadinimas = "sandėliavimas (EVRK 63.1)"
                //    });
                //    session.Save(new ProduktuPardavimoRusis()
                //    {
                //        Kodas = "600",
                //        Pavadinimas = "Iš viso (180, 390, 400, 500 kodų suma)"
                //    });

                //    session.Save(new DotacijuSubsidijuRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Dotacijos ir subsidijos, negautoms pajamoms kompensuoti 1"
                //    });
                //    session.Save(new DotacijuSubsidijuRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "už pasėlius ir žemės ūkio naudmenis"
                //    });
                //    session.Save(new DotacijuSubsidijuRusis()
                //    {
                //        Kodas = "012",
                //        Pavadinimas = "už gyvulius"
                //    });
                //    session.Save(new DotacijuSubsidijuRusis()
                //    {
                //        Kodas = "013",
                //        Pavadinimas = "kitos kompensacinės išmokos negautoms pajamoms kompensuoti"
                //    });
                //    session.Save(new DotacijuSubsidijuRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Dotacijos ir subsidijos patirtoms išlaidoms kompensuoti 2"
                //    });
                //    session.Save(new DotacijuSubsidijuRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Dotacijos, susijusios su turtu 3"
                //    });

                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Grūdai (po apdorojimo)"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "iš jų: žieminiai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "012",
                //        Pavadinimas = "vasariniai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "013",
                //        Pavadinimas = "ankštiniai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Rapsai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "021",
                //        Pavadinimas = "iš jų: žieminiai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "022",
                //        Pavadinimas = "vasariniai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Linai: sėmenys"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "šiaudeliai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "Cukriniai runkeliai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "070",
                //        Pavadinimas = "Pašariniai šakniavaisiai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "080",
                //        Pavadinimas = "Bulvės"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "090",
                //        Pavadinimas = "Atviro grunto daržovės"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "100",
                //        Pavadinimas = "Uždaro grunto daržovės"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "110",
                //        Pavadinimas = "Vaisiai (sėklavaisiai, kaulavaisiai)"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "120",
                //        Pavadinimas = "Uogos (braškės, serbentai ir kitos)"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "130",
                //        Pavadinimas = "Daugiametės žolės šienui  "
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "140",
                //        Pavadinimas = "žaliajai masei"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "150",
                //        Pavadinimas = "Vienmetės žolės šienui"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "160",
                //        Pavadinimas = "Pievos ir ganyklos"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "170",
                //        Pavadinimas = "Silosas"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "180",
                //        Pavadinimas = "Šienainis"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "190",
                //        Pavadinimas = "Lauko gėlininkystė"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "200",
                //        Pavadinimas = "Šiltnamių gėlininkystė"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "210",
                //        Pavadinimas = "Pievagrybiai"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "220",
                //        Pavadinimas = "Kita produkcija"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "230",
                //        Pavadinimas = "Išlaidos kitų metų derliui"
                //    });
                //    session.Save(new AugalininkystesRusis()
                //    {
                //        Kodas = "240",
                //        Pavadinimas = "IŠ VISO"
                //    });

                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Galvijininkystė Pagrindinės bandos produkcija"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "pienas "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "012",
                //        Pavadinimas = "prievaisa vnt."
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Galvijų prieauglio ir penimų gyvulių produkcija"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "021",
                //        Pavadinimas = "priesvoris "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Kiaulininkystė, iš viso"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "031",
                //        Pavadinimas = "iš jų priesvoris "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Avininkystė "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "041",
                //        Pavadinimas = "iš jų:    priesvoris "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "042",
                //        Pavadinimas = "vilna "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Paukštininkystė Vištų produkcija, iš viso"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "051",
                //        Pavadinimas = "iš jų:    kiaušiniai tūkst. vnt."
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "052",
                //        Pavadinimas = "priesvoris "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "Kitų paukščių produkcija, iš viso"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "061",
                //        Pavadinimas = "iš jų:    kiaušiniai tūkst. vnt."
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "062",
                //        Pavadinimas = "priesvoris "
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "080",
                //        Pavadinimas = "Arklininkystė, iš viso"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "090",
                //        Pavadinimas = "Bitininkystė, iš viso"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "091",
                //        Pavadinimas = "t. sk.    medus kg"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "092",
                //        Pavadinimas = "vaškas kg"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "100",
                //        Pavadinimas = "Žuvininkystė"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "110",
                //        Pavadinimas = "Kita produkcija"
                //    });
                //    session.Save(new GyvulininkystesRusis()
                //    {
                //        Kodas = "120",
                //        Pavadinimas = "Iš viso"
                //    });

                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Grūdai, iš viso"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "iš jų: kviečiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "012",
                //        Pavadinimas = "rugiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "013",
                //        Pavadinimas = "kvietrugiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "014",
                //        Pavadinimas = "miežiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "015",
                //        Pavadinimas = "avižos"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "016",
                //        Pavadinimas = "žirniai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "017",
                //        Pavadinimas = "vikiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "018",
                //        Pavadinimas = "lubinai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "019",
                //        Pavadinimas = "pupos"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "grikiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "021",
                //        Pavadinimas = "javų mišiniai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "022",
                //        Pavadinimas = "kukurūzai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Rapsai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Cukriniai runkeliai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Bulvės"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "Daržovės"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "070",
                //        Pavadinimas = "Pašariniai šakniavaisiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "080",
                //        Pavadinimas = "Linų sėmenys"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "090",
                //        Pavadinimas = "Linų šiaudeliai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "100",
                //        Pavadinimas = "Vaisiai ir uogos"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "110",
                //        Pavadinimas = "Gėlės"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "120",
                //        Pavadinimas = "Pievagrybiai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "130",
                //        Pavadinimas = "Miltai ir kiti grūdų perdirbimo produktai"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "140",
                //        Pavadinimas = "Pienas"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "150",
                //        Pavadinimas = "Mėsa skerdienos svoriu"
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "160",
                //        Pavadinimas = "Paukščių kiaušiniai tūkst. vnt."
                //    });
                //    session.Save(new ProdukcijosKaitosRusis()
                //    {
                //        Kodas = "170",
                //        Pavadinimas = "Medus"
                //    });

                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Galvijai"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "011",
                //        Pavadinimas = "iš jų:   melžiamos karvės"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "012",
                //        Pavadinimas = "karvės žindenės"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "013",
                //        Pavadinimas = "buliai reproduktoriai"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Kiaulės"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "021",
                //        Pavadinimas = "iš jų    paršavedės"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "Avys ir ožkos"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "031",
                //        Pavadinimas = "iš jų    vedeklės"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "Visų amžių grupių paukščiai tūkst. vnt."
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "041",
                //        Pavadinimas = "iš jų    dedeklės"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "Arkliai"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "051",
                //        Pavadinimas = "iš jų   suaugę"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "Triušiai"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "070",
                //        Pavadinimas = "Švelniakailiai žvėreliai"
                //    });
                //    session.Save(new GyvuliuSkaiciausRusis()
                //    {
                //        Kodas = "080",
                //        Pavadinimas = "Bičių šeimos"
                //    });

                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "010",
                //        Pavadinimas = "Bendras žemės plotas"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "020",
                //        Pavadinimas = "Iš viso ž. ū. naudmenų"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "030",
                //        Pavadinimas = "iš jų:   ariama žemė"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "040",
                //        Pavadinimas = "sodai ir uogynai"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "050",
                //        Pavadinimas = "iš jų derančio amžiaus"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "060",
                //        Pavadinimas = "pievos ir ganyklos"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "070",
                //        Pavadinimas = "Kita žemė"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "080",
                //        Pavadinimas = "Miškai"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "090",
                //        Pavadinimas = "Nusausinta žemė"
                //    });
                //    session.Save(new ZemesPlotuRusis()
                //    {
                //        Kodas = "100",
                //        Pavadinimas = "Drėkinama žemė"
                //    });

                //    transaction.Commit();
                //}

                Log.Info("Done!");
            }
            catch(Exception ex)
            {
                Log.Fatal(ex);
            }

            Console.ReadLine();
        }
    }
}
