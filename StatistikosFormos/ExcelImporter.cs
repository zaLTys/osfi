using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using NHibernate.Linq;
using Vic.ZubStatistika.DataAccess;
using Vic.ZubStatistika.Entities;
using System.Collections.ObjectModel;
using NHibernate;

namespace StatistikosFormos
{
    public class ExcelImporter
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ISessionFactory _sessionFactory;

        public ExcelImporter(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Import(string excelFile, int metai)
        {

            var excelConnectonString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;IMEX=1;MAXSCANROWS=0;HDR=No\"", excelFile);

            using (var excelConnection = new OleDbConnection(excelConnectonString))
            {
                try
                {
                    excelConnection.Open();
                }
                catch (Exception ex)
                {
                    throw new ImportException("Negalima atidaryti excelio bylos. Galimos priežastys: excelio byla atidaryta kitoje programoje arba atidaroma byla yra ne excelio byla.", ex);
                }
                
                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var imonesRekvizitai = GetImonesRekvizitai(excelConnection);

                    var imone = session.Query<Imone>()
                        .FirstOrDefault(i => i.AsmensKodas == imonesRekvizitai.AsmensKodas);

                    if (imone != null)
                    {
                        Log.DebugFormat("Imone, kurios kodas {0}, jau egzistavo duomenu bazeje", imonesRekvizitai.AsmensKodas);
                    }
                    else
                    {
                        imone = new Imone()
                        {
                            AsmensKodas = imonesRekvizitai.AsmensKodas
                        };
                    }

                    session.SaveOrUpdate(imone);

                    var now = DateTime.Now;

                    var upload = imone.CreateNepatvirtintasUpload(metai, now);
                    imone.AddImonesDuomenys(upload, new ImonesDuomenys
                                                       {
                                                           Adresas = imonesRekvizitai.Adresas,
                                                           AtaskaitosDuomenys = imonesRekvizitai.AtaskaitosDuomenys,
                                                           ElPastas = imonesRekvizitai.ElPastas,
                                                           FormuPildymoData = imonesRekvizitai.FormuPildymoData,
                                                           ImonesFinansininkas = imonesRekvizitai.ImonesFinansininkas,
                                                           ImonesVadovas = imonesRekvizitai.ImonesVadovas,
                                                           Pavadinimas = imonesRekvizitai.Pavadinimas,
                                                           Telefonas = imonesRekvizitai.Telefonas,                                                           
                                                       });
                    session.Save(imone);

                    ImportExcelioLentele<IlgalaikisTurtas, IlgalaikioTurtoRusis>(session, excelConnection, imone, upload, (stulpelis, turtas) =>
                    {
                        int i = 1;

                        try
                        {
                            turtas.LikutisPradziojeIlgalaikio = stulpelis[i++].AsDecimal();
                            turtas.Gauta = stulpelis[i++].AsDecimal();
                            turtas.IsJuNauju = stulpelis[i++].AsDecimal();
                            turtas.VertesPadidejimas = stulpelis[i++].AsDecimal();
                            turtas.NurasytaIlgalaikio = stulpelis[i++].AsDecimal();
                            turtas.LikviduotaIlgalaikio = stulpelis[i++].AsDecimal();
                            turtas.ParduotaIlgalaikio = stulpelis[i++].AsDecimal();
                            turtas.Nukainota = stulpelis[i++].AsDecimal();
                            turtas.LikutisPabaigojeIlgalaikio = stulpelis[i++].AsDecimal();
                            turtas.LikutisPradziojeNusidevejimo = stulpelis[i++].AsDecimalX();
                            turtas.Priskaiciuota = stulpelis[i++].AsDecimalX();
                            turtas.Pasikeitimas = stulpelis[i++].AsDecimalX();
                            turtas.NurasytaNusidevejimo = stulpelis[i++].AsDecimalX();
                            turtas.LikviduotaNusidevejimo = stulpelis[i++].AsDecimalX();
                            turtas.LikutisPabaigojeNusidevejimo = stulpelis[i++].AsDecimalX();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F1\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma1");

                    ImportExcelioLentele<Augalininkyste, AugalininkystesRusis>(session, excelConnection, imone, upload, (stulpelis, augal) =>
                    {
                        try
                        {
                            int i = 1;
                            augal.Plotas = stulpelis[i++].AsDecimalX();
                            augal.ProdukcijosKiekis = stulpelis[i++].AsDecimalX();
                            augal.Derlingumas = stulpelis[i++].AsDecimalX();
                            augal.IslaidosDarboApmokejimas = stulpelis[i++].AsDecimal();
                            augal.IslaidosSeklos = stulpelis[i++].AsDecimal();
                            augal.IslaidosTrasos = stulpelis[i++].AsDecimal();
                            augal.IslaidosNafta = stulpelis[i++].AsDecimal();
                            augal.IslaidosElektra = stulpelis[i++].AsDecimal();
                            augal.IslaidosKitos = stulpelis[i++].AsDecimal();
                            augal.IslaidosVisos = stulpelis[i++].AsDecimal();
                            augal.IslaidosPagrindinei = stulpelis[i++].AsDecimalX();
                            augal.ProdukcijosSavikaina = stulpelis[i++].AsDecimalX();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F5\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma5");

                    ImportExcelioLentele<Darbuotojai, DarbuotojuRusis>(session, excelConnection, imone, upload, (stulpelis, darbuot) =>
                    {
                        try
                        {
                            int i = 1;
                            darbuot.Reiksme = stulpelis[i++].AsDecimal();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F2\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma2");

                    ImportExcelioLentele<Sanaudos, SanauduRusis>(session, excelConnection, imone, upload, (stulpelis, sanaud) =>
                    {
                        try
                        {
                            int i = 1;
                            sanaud.IsViso = stulpelis[i++].AsDecimal();
                            sanaud.Augalininkyste = stulpelis[i++].AsDecimalX();
                            sanaud.Gyvulininkyste = stulpelis[i++].AsDecimalX();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F3\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma3");

                    ImportExcelioLentele<ProduktuPardavimas, ProduktuPardavimoRusis>(session, excelConnection, imone, upload, (stulpelis, prodpard) =>
                    {
                        try
                        {
                            int i = 1;
                            prodpard.ParduotaNatura = stulpelis[i++].AsDecimalX();
                            prodpard.ParduotaEksportui = stulpelis[i++].AsDecimalX();
                            prodpard.ParduotaIskaitomuojuSvoriu = stulpelis[i++].AsDecimalX();
                            prodpard.ProdukcijosSavikaina = stulpelis[i++].AsDecimal();
                            prodpard.PardavimuPajamos = stulpelis[i++].AsDecimal();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F4.1\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }

                    }, "Forma41augalai", "Forma41Gyvunai", "Forma41Kita");

                    ImportExcelioLentele<DotacijosSubsidijos, DotacijuSubsidijuRusis>(session, excelConnection, imone, upload, (stulpelis, dota) =>
                    {
                        try
                        {
                            int i = 1;
                            dota.Suma = stulpelis[i++].AsDecimal();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F4.2\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }

                    }, "Forma42straipsniai");

                    ImportExcelioLentele<Gyvulininkyste, GyvulininkystesRusis>(session, excelConnection, imone, upload, (stulpelis, gyvul) =>
                    {
                        try
                        {
                            int i = 1;
                            gyvul.VidutinisGyvuliuSk = stulpelis[i++].AsDecimalX();
                            gyvul.ProdukcijosKiekis = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosDarboApmokejimas = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosPasarai = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosNafta = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosElektra = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosKitos = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosVisos = stulpelis[i++].AsDecimalX();
                            gyvul.IslaidosPagrindinei = stulpelis[i++].AsDecimalX();
                            gyvul.ProdukcijosSavikaina = stulpelis[i++].AsDecimalX();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F6\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma6");

                    ImportExcelioLentele<ProdukcijosKaita, ProdukcijosKaitosRusis>(session, excelConnection, imone, upload, (stulpelis, prodkait) =>
                    {
                        try
                        {
                            int i = 1;
                            prodkait.MetuPradziosLikutis = stulpelis[i++].AsDecimal();
                            prodkait.PajamosPagaminta = stulpelis[i++].AsDecimal();
                            prodkait.PajamosPirkta = stulpelis[i++].AsDecimal();
                            prodkait.PajamosImportuota = stulpelis[i++].AsDecimal();
                            prodkait.IslaidosVisos = stulpelis[i++].AsDecimal();
                            prodkait.IslaidosParduota = stulpelis[i++].AsDecimal();
                            prodkait.IslaidosPasarui = stulpelis[i++].AsDecimal();
                            prodkait.IslaidosSeklai = stulpelis[i++].AsDecimalX();
                            prodkait.IslaidosDuotaPerdirbti = stulpelis[i++].AsDecimal();
                            prodkait.IslaidosProdukcijosNuostoliai = stulpelis[i++].AsDecimal();
                            prodkait.IslaidosKitos = stulpelis[i++].AsDecimal();
                            prodkait.MetuPabaigosLikutis = stulpelis[i++].AsDecimal();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F7\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma7");

                    ImportExcelioLentele<GyvuliuSkaicius, GyvuliuSkaiciausRusis>(session, excelConnection, imone, upload, (stulpelis, gyvsk) =>
                    {
                        try
                        {
                            int i = 1;
                            gyvsk.MetuPradzioje = stulpelis[i++].AsDecimal();
                            gyvsk.MetuPabaigojeVnt = stulpelis[i++].AsDecimal();
                            gyvsk.MetuPabaigojeVerte = stulpelis[i++].AsDecimal();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F8\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma8");

                    ImportExcelioLentele<ZemesPlotai, ZemesPlotuRusis>(session, excelConnection, imone, upload, (stulpelis, zem) =>
                    {
                        try
                        {
                            int i = 1;
                            zem.NuomaIsValstybes = stulpelis[i++].AsDecimal();
                            zem.NuomaIsFiziniu = stulpelis[i++].AsDecimal();
                            zem.NuosavaZeme = stulpelis[i++].AsDecimal();
                        }
                        catch (Exception ex)
                        {
                            throw new ImportException("Nepavyko įkelti lentelės \"F9\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
                        }
                    }, "Forma9");

                    ImportLaikai(session, excelConnection, imone, upload);

                    try
                    {
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        throw new ImportException("Nepavyko išsaugoti informacijos duomenų bazėje.", ex);
                    }
                }
            }
        }

        private static ImonesRekvizitai GetImonesRekvizitai(OleDbConnection excelConnection)
        {
            try
            {
                var dataAdapter = new OleDbDataAdapter("SELECT * FROM Rekvizitai2", excelConnection);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                return new ImonesRekvizitai
                           {
                               AsmensKodas = Convert.ToInt64(dataTable.Rows[0][3]),
                               Pavadinimas = dataTable.Rows[1][3].ToString(),
                               FormuPildymoData = dataTable.Rows[2][3].ToString(),
                               ImonesVadovas = dataTable.Rows[3][3].ToString(),
                               ImonesFinansininkas = dataTable.Rows[4][3].ToString(),
                               Adresas = dataTable.Rows[5][3].ToString(),
                               ElPastas = dataTable.Rows[6][3].ToString(),
                               Telefonas = dataTable.Rows[7][3].ToString(),
                               AtaskaitosDuomenys = dataTable.Rows[8][3].AsInt(),
                           };
            }
            catch (Exception ex)
            {
                throw new ImportException("Nepavyko įkelti įmonės duomenų. Patikrinkite, ar jie įvesti teisingai.", ex);
            }
        }

        private static void ImportExcelioLentele<TLentele, TKlasifikatorius>(ISession session, OleDbConnection excelConnection, Imone imone, Upload upload, Action<DataRow, TLentele> importoVeiksmas, params string[] namedBlocks)
            where TLentele : class, IExcelioLentele<TKlasifikatorius>, new()
            where TKlasifikatorius : class, IKlasifikatorius
        {
            foreach (var namedBlock in namedBlocks)
            {
                var dataAdapter = new OleDbDataAdapter(string.Format("SELECT * FROM {0}", namedBlock), excelConnection);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                var irasuTipai = session.Query<TKlasifikatorius>().ToList();

                foreach (DataRow stulpelis in dataTable.Rows)
                {
                    if (stulpelis[0] == DBNull.Value) continue;
                    var teisingasTipas = irasuTipai.Single(x => x.Kodas == stulpelis[0].ToString());
                    Log.DebugFormat("Irasome lenteles irasa tipo {0}", teisingasTipas);

                    var irasas = new TLentele()
                    {
                        Upload = upload,
                        Rusis = teisingasTipas,
                        Imone = imone,
                    };

                    int i = 1;

                    importoVeiksmas(stulpelis, irasas);

                    session.Save(irasas);
                }
            }
        }

        private static void ImportLaikai(ISession session, OleDbConnection excelConnection, Imone imone, Upload upload)
        {
            try
            {
                var dataAdapter = new OleDbDataAdapter("SELECT * FROM Forma42laikas", excelConnection);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                var irasas = new FormosPildymoLaikas()
                {
                    Upload = upload,
                    Imone = imone,
                    Minutes = dataTable.Rows[0][1].AsInt(),
                    Valandos = dataTable.Rows[0][0].AsInt()
                };

                session.Save(irasas);
            }
            catch (Exception ex)
            {
                throw new ImportException("Nepavyko įkelti lentelės \"F4.2\". Patikrinkite, ar šios lentelės duomenys įvesti teisingai.", ex);
            }
        }
    }
}
