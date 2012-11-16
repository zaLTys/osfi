﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;
using Oracle.DataAccess.Client;
using PetaPoco;
using StatistinesAtaskaitos.Models;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Services
{
    public class ImoniuService : IImoniuService
    {
        private readonly ISessionFactory _sessionFactory;

        public ImoniuService(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<ImoneGridModel> GetImones(int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var imones = from u in session.Query<Upload>()
                             join bukle in session.Query<UploadStatus>() on u.Id equals bukle.Upload.Id
                             join imonesDuomenys in session.Query<ImonesDuomenys>() on u.Id equals imonesDuomenys.Upload.Id
                             where u.Metai == metai
                              && bukle.DataIki == null
                              && bukle.Bukle != "Netinkamas"
                             select new
                             {
                                 u.Imone.Id,
                                 u.Imone.AsmensKodas,
                                 imonesDuomenys.Pavadinimas,
                                 bukle.Bukle
                             };

                var groupedImones = from i in imones.AsEnumerable()
                                    group i by new { i.Id, i.AsmensKodas, i.Pavadinimas }
                                    into g select new
                                    {
                                        Id = g.Key.Id,
                                        AsmensKodas = g.Key.AsmensKodas,
                                        Pavadinimas = g.Key.Pavadinimas,
                                        PatvirtintuSkaicius = g.Count(x => x.Bukle == "Patvirtintas"),
                                        NepatvirtintuSkaicius = g.Count(x => x.Bukle == "Nepatvirtintas"),
                                        AtmestuSkaicius = g.Count(x => x.Bukle == "Atmestas")
                                    };

                return groupedImones
                    .Select(x => new ImoneGridModel
                                     {
                                         Id = x.Id,
                                         AsmensKodas = x.AsmensKodas,
                                         Pavadinimas = x.Pavadinimas,
                                         Statusas = (x.NepatvirtintuSkaicius > 0 ? ImonesStatusas.LaukiamaPatvirtinimo : x.PatvirtintuSkaicius > 0 ? ImonesStatusas.Patvirtinta : ImonesStatusas.Atmesta)
                                     })
                    .OrderBy(x => x.Pavadinimas)
                    .ToList();
            }
        }

        public ImoneDetailsModel GetImoneDetails(int imonesId, int metai)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var imone = session.Query<Imone>().FirstOrDefault(x => x.Id == imonesId);
                if (imone == null) return null;

                var imoneUploads = (from u in session.Query<Upload>()
                                   join bukle in session.Query<UploadStatus>() on u.Id equals bukle.Upload.Id
                                   where u.Metai == metai
                                         && u.Imone.Id == imonesId
                                         && bukle.DataIki == null
                                         && bukle.Bukle != "Netinkamas"
                                   orderby bukle.DataNuo
                                   select new ImoneDetailsUploadModel
                                              {
                                                  Id = u.Id,
                                                  Bukle = bukle.Bukle,
                                                  BuklesData = bukle.DataNuo,
                                                  Data = u.Data,
                                                  Metai = u.Metai
                                              }).ToList();

                var latestUpload = imoneUploads.Last();

                return new ImoneDetailsModel()
                           {
                               Id = imone.Id,
                               AsmensKodas = imone.AsmensKodas,
                               Pavadinimas = imone.Duomenys.First(x => x.Upload.Id == latestUpload.Id).Pavadinimas,
                               Ikelimai = imoneUploads,
                           };

            }
        }
    }
}