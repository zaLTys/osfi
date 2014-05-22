using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.DataAccess;
using StatistinesAtaskaitos.Models;
using Oracle.DataAccess.Client;

namespace StatistinesAtaskaitos.Services
{
//    public class TheRealStatistiniuAtaskaituService : IStatistiniuAtaskaituService
//    {
//        public IEnumerable<Forma11> GetForma1(long imonesKodas)
//        {
//            const string oradb = @"Data Source=(DESCRIPTION=(ADDRESS_LIST=
//                                  (ADDRESS=(PROTOCOL=TCP)(HOST=vld.vic.lt)(PORT=1521)))
//                                  (CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=vld.vic.lt)));
//                                  User Id=valduskyrius;Password=hh2upcgp_;";

//            using(var conn = new OracleConnection(oradb))
//            {
//                conn.Open();
//                using (var cmd = new OracleCommand())
//                {
//                    cmd.Connection = conn;
//                    cmd.CommandText =
//                        @"select
//                            itr.pavadinimas pavadinimas
//                            ,itr.kodas kodas
//                            ,sum(it.likutispradziojeilgalaikio/1) likutispradziojeilgalaikio
//                            ,sum(it.gauta/1) gauta
//                            ,sum(it.isjunauju/1) isjunauju
//                            ,sum(it.vertespadidejimas/1) vertespadidejimas
//                            ,sum(it.nurasytailgalaikio/1) nurasytailgalaikio
//                            ,sum(it.likviduotailgalaikio/1) likviduotailgalaikio
//                            ,sum(it.parduotailgalaikio/1) parduotailgalaikio
//                            ,sum(it.nukainota/1) nukainota
//                            ,sum(it.likutispabaigojeilgalaikio/1) likutispabaigojeilgalaikio
//                            ,sum(it.likutispradziojenusidevejimo/1) likutispradziojenusidevejimo
//                            ,sum(it.priskaiciuota/1) priskaiciuota
//                            ,sum(it.pasikeitimas/1) pasikeitimas
//                            ,sum(it.nurasytanusidevejimo/1) nurasytanusidevejimo
//                            ,sum(it.likviduotanusidevejimo/1) likviduotanusidevejimo
//                            ,sum(it.likutispabaigojenusidevejimo/1) likutispabaigojenusidevejimo
//                            from
//                            osfi.ilgalaikioturtorusis itr
//                            ,osfi.ilgalaikisturtas it
//                            ,osfi.imone im
//                            where
//                            it.dataiki is null
//                            and it.rusis_id = itr.id
//                            and it.imone_id = im.id
//                            and im.asmenskodas in(:imoneskodas)
//                            group by
//                            itr.kodas
//                            ,itr.pavadinimas
//                            order by 2";

//                    cmd.CommandType = CommandType.Text;
//                    cmd.Parameters.Add("imoneskodas", imonesKodas);
//                    using (var dr = cmd.ExecuteReader())
//                    {
//                        while (dr.Read())
//                        {
//                            var imone = new Forma11();
//                            imone.Pavadinimas = dr.ReadValue<string>("pavadinimas"); //dr["pavadinimas"].ToString();
//                            imone.Kodas = dr.ReadValue<string>("kodas"); // dr["kodas"].ToString();
//                            imone.LikutisPradziojeIlgalaikio = dr.ReadValue<decimal>("likutispradziojeilgalaikio"); //Convert.Todecimal(dr["likutispradziojeilgalaikio"]);
//                            imone.Pavadinimas = dr.ReadValue<string>("pavadinimas"); // dr["pavadinimas"].ToString();
//                            imone.Gauta = dr.ReadValue<decimal>("gauta"); //Convert.Todecimal(dr["gauta"]));
//                            imone.IsJuNauju = dr.ReadValue<decimal>("isjunauju"); // Convert.Todecimal(dr["isjunauju"]);
//                            imone.VertesPadidejimas = dr.ReadValue<decimal>("vertespadidejimas"); //Convert.Todecimal(dr["vertespadidejimas"]);
//                            imone.NurasytaIlgalaikio = dr.ReadValue<decimal>("nurasytailgalaikio"); //Convert.Todecimal(dr["nurasytailgalaikio"]);
//                            imone.LikviduotaIlgalaikio = dr.ReadValue<decimal>("likviduotailgalaikio"); //Convert.Todecimal(dr["likviduotailgalaikio"]);
//                            imone.ParduotaIlgalaikio = dr.ReadValue<decimal>("parduotailgalaikio"); //Convert.Todecimal(dr["parduotailgalaikio"]);
//                            imone.Nukainota = dr.ReadValue<decimal>("nukainota"); // Convert.Todecimal(dr["nukainota"]);
//                            imone.LikutisPabaigojeIlgalaikio = dr.ReadValue<decimal>("likutispabaigojeilgalaikio"); //Convert.Todecimal(dr["likutispabaigojeilgalaikio"]);
//                            imone.LikutisPradziojeNusidevejimo = dr.ReadNullableValue<decimal>("likutispradziojenusidevejimo");
//                            imone.Priskaiciuota = dr.ReadNullableValue<decimal>("priskaiciuota");
//                            imone.Pasikeitimas = dr.ReadNullableValue<decimal>("pasikeitimas");
//                            imone.NurasytaNusidevejimo = dr.ReadNullableValue<decimal>("nurasytanusidevejimo");
//                            imone.LikviduotaNusidevejimo = dr.ReadNullableValue<decimal>("likviduotanusidevejimo");
//                            imone.LikutisPabaigojeNusidevejimo = dr.ReadNullableValue<decimal>("likutispabaigojenusidevejimo");

//                            //if (dr["likutispradziojenusidevejimo"] != DBNull.Value)
//                            //    imone.LikutisPradziojeNusidevejimo = Convert.Todecimal(dr["likutispradziojenusidevejimo"]);
//                            //if (dr["priskaiciuota"] != DBNull.Value)
//                            //    imone.Priskaiciuota = Convert.Todecimal(dr["priskaiciuota"]);
//                            //if (dr["pasikeitimas"] != DBNull.Value)
//                            //    imone.Pasikeitimas = Convert.Todecimal(dr["pasikeitimas"]);
//                            //if (dr["nurasytanusidevejimo"] != DBNull.Value)
//                            //    imone.NurasytaNusidevejimo = Convert.Todecimal(dr["nurasytanusidevejimo"]);
//                            //if (dr["likviduotanusidevejimo"] != DBNull.Value)
//                            //    imone.LikviduotaNusidevejimo = Convert.Todecimal(dr["likviduotanusidevejimo"]);
//                            //if (dr["likutispabaigojenusidevejimo"] != DBNull.Value)
//                            //imone.LikutisPabaigojeNusidevejimo = Convert.Todecimal(dr["likutispabaigojenusidevejimo"]);
                            
//                            yield return imone;
//                        }
//                    }
//                }
//            }
//        }


//        IEnumerable<Forma2> IStatistiniuAtaskaituService.GetForma2(long imonesKodas)
//        {
//            throw new NotImplementedException();
//        }
//        IEnumerable<Forma3> IStatistiniuAtaskaituService.GetForma3(long imonesKodas)
//        {
//            throw new NotImplementedException();
//        }
//        IEnumerable<Forma41> IStatistiniuAtaskaituService.GetForma41(long imonesKodas)
//        {
//            throw new NotImplementedException();
//        }
//        IEnumerable<Forma42> IStatistiniuAtaskaituService.GetForma42(long imonesKodas)
//        {
//            throw new NotImplementedException();
//        }

//    }
}