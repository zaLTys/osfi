using System;
using System.Collections.Generic;
using System.Linq;
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

        public VartotojuAdministravimasController(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
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
                                         Role = x.Role,
                                     })
                    .ToList();
                return View(users);
            }
            
        }

    }
}
