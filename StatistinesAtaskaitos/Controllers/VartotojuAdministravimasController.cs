using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using StatistinesAtaskaitos.Models;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Controllers
{
    [Authorize(Roles = "Administratorius")]
    public class VartotojuAdministravimasController : Controller
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly HashAlgorithm _hashAlgorithm;

        public VartotojuAdministravimasController(ISessionFactory sessionFactory, HashAlgorithm hashAlgorithm)
        {
            _sessionFactory = sessionFactory;
            _hashAlgorithm = hashAlgorithm;
        }


        public ActionResult Index()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<User>()
                    .Select(x => new UserModel
                                     {
                                         Id = x.Id,
                                         Username = x.Username,
                                         Vardas = x.Vardas,
                                         Pavarde = x.Pavarde,
                                         Role = (RoliuTipai)Enum.Parse(typeof(RoliuTipai), x.Role),
                                     })
                    .ToList();
                return View(users);
            }
            
        }

        [HttpGet]
        public ActionResult Redagavimas(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Query<User>().First(x => x.Id == id);
                return View(new UserEditModel
                                {
                                    Id = user.Id,
                                    Pavarde = user.Pavarde,
                                    Role =(RoliuTipai) Enum.Parse(typeof(RoliuTipai),user.Role),
                                    Username = user.Username,
                                    Vardas = user.Vardas
                                });
            }
            
        }

        [HttpPost]
        public ActionResult Redagavimas(UserEditModel gediminas)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Query<User>().First(x => x.Id == gediminas.Id);
                user.Role = gediminas.Role.ToString();
                user.Vardas = gediminas.Vardas;
                user.Pavarde = gediminas.Pavarde;

                session.Update(user);
                transaction.Commit();

                return RedirectToAction("Index");
            }

        }

        [HttpGet]
        public ActionResult Kurimas()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                return View(new UserCreateModel());
            }

        }

        [HttpPost]
        public ActionResult Kurimas(UserCreateModel gediminas)
        {
            if (!ModelState.IsValid) return View(gediminas);

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = new User
                               {
                                   Username = gediminas.Username,
                                   Password = _hashAlgorithm.GetHashedString(gediminas.Password),
                                   Role = gediminas.Role.ToString(),
                                   Vardas = gediminas.Vardas,
                                   Pavarde = gediminas.Pavarde
                               };

                session.Persist(user);
                transaction.Commit();

                return RedirectToAction("Index");
            }

        }
 
        public ActionResult Trinti(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Query<User>().First(x => x.Id == id);
                session.Delete(user);
                transaction.Commit();

                return RedirectToAction("Index");
            }

        }

    }
}
