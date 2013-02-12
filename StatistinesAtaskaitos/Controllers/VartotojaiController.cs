using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Security;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Controllers
{
    public class VartotojaiController : Controller
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly ISessionFactory _sessionFactory;
        private readonly UserInformation _loggedInUser;
        private readonly HashAlgorithm _hashAlgorithm;

        public VartotojaiController(IAuthenticationProvider authenticationProvider, ISessionFactory sessionFactory, [LoggedIn] UserInformation loggedInUser, HashAlgorithm hashAlgorithm)
        {
            _authenticationProvider = authenticationProvider;
            _sessionFactory = sessionFactory;
            _loggedInUser = loggedInUser;
            _hashAlgorithm = hashAlgorithm;
        }

        //
        // GET: /Vartotojai/
        [HttpGet]
        public ActionResult Prisijungimas()
        {
            return View(new PrisijungimoForma
                            {
                                ReturnUrl = Request.QueryString["ReturnUrl"]
                            });
        }

        [HttpPost]
        public ActionResult Prisijungimas(PrisijungimoForma forma)
        {
            if (_authenticationProvider.LogIn(forma.VartotojoVardas, forma.Slaptazodis, forma.Prisiminti))
            {
                var returnUrl = "~/";
                if (!string.IsNullOrEmpty(forma.ReturnUrl)) returnUrl = forma.ReturnUrl;

                return this.Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Neteisingas vartotojo vardas arba slaptažodis.");
            }

            return View(forma);
        }
        
        public ActionResult Atsijungti()
        {
            _authenticationProvider.LogOut();
            return this.Redirect("~/");
        }

        [HttpGet]
        public ActionResult KeistiSlaptazodi()
        {
            return View(new SlaptazodzioKeitimasModel());
        }

        [HttpPost]
        public ActionResult KeistiSlaptazodi(SlaptazodzioKeitimasModel forma)
        {
            if (!ModelState.IsValid) return View(new SlaptazodzioKeitimasModel());

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Query<User>().First(x => x.Id == _loggedInUser.Id);
                user.Password = _hashAlgorithm.GetHashedString(forma.NaujasSlaptazodis);

                session.Update(user);
                transaction.Commit();

                return Redirect("~/");
            }
        }
    }
}
