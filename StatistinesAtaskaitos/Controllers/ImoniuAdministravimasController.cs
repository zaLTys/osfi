using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Services;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Controllers
{
    [Authorize(Roles = "Administratorius")]
    public class ImoniuAdministravimasController : Controller
    {
        private readonly IImoniuService _imoniuService;
        private readonly ISessionFactory _sessionFactory;

        public ImoniuAdministravimasController(IImoniuService imoniuService, ISessionFactory sessionFactory)
        {
            _imoniuService = imoniuService;
            _sessionFactory = sessionFactory;
        }

        //
        // GET: /ImoniuAdministravimas/

        public ActionResult Index(int? metai)
        {
            metai = metai ?? DateTime.Now.Year;

            var model = new DuomenisPateikusiosImonesModel
                            {
                                Metai = metai.Value,
                                Imones = _imoniuService.GetImones(metai.Value)
                            };

            return View(model);
        }

        public ActionResult Details(int id, int metai)
        {
            var imoneDetails = _imoniuService.GetImoneDetails(id, metai);
            return View(imoneDetails);
        }

        public ActionResult Patvirtinti(int imoneId, int metai, int uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var imone = session.Query<Imone>().First(x => x.Id == imoneId);
                var user = session.Query<User>().First(x => x.Id == 1);
                imone.PatvirtintiUploada(user, uploadId, DateTime.Now);

                session.Save(imone);
                tx.Commit();
            }

            return RedirectToAction("Details", new {id = imoneId, metai = metai});
        }

        public ActionResult Atmesti(int imoneId, int metai, int uploadId)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                var imone = session.Query<Imone>().First(x => x.Id == imoneId);
                var user = session.Query<User>().First(x => x.Id == 1);
                
                imone.AtmestiUploada(user, uploadId, DateTime.Now);

                session.Save(imone);
                tx.Commit();
            }

            return RedirectToAction("Details", new { id = imoneId, metai = metai });
        }
    }
}
