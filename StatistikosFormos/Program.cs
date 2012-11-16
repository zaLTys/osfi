using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Vic.ZubStatistika.DataAccess;
using System.Data.Entity;
using System.Configuration;
using Vic.ZubStatistika.Entities;

namespace StatistikosFormos
{
    class Program
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();

            try
            {
                var connString = ConfigurationManager.ConnectionStrings["FormosDuombaze"].ConnectionString;

                var sessionFactory = Fluently.Configure()
                    .Database(OracleDataClientConfiguration.Oracle10
                                  .ConnectionString(connString).DefaultSchema("OSFI"))
                    .Mappings(m =>
                                  {
                                      m.AutoMappings.Add(
                                          AutoMap.AssemblyOf<Augalininkyste>().Conventions.Add(
                                              new NHibernateSequenceConvention(), Table.Is(x => x.TableName.ToUpper())));

                                  })
                    .BuildSessionFactory();

                var fileName = args.FirstOrDefault(x => x.StartsWith("file="));
                if (fileName == null)
                {
                    Log.Info("Nenurodytas bylos parametras. Importuojamą bylą galite nurodyti parametru \"file=<bylos vardas be tarpų>\".");
                    return;
                }

                fileName = fileName.Substring("file=".Length);

                var importer = new ExcelImporter(sessionFactory);
                importer.Import(fileName, 2012);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }

            Console.ReadLine();
        }
    }
}
