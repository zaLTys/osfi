using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatistinesAtaskaitos.Security;

namespace StatistinesAtaskaitos.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserInformation _loggedInUser;

        public HomeController([LoggedIn] UserInformation loggedInUser)
        {
            _loggedInUser = loggedInUser;
        }

        public ActionResult Index()
        {
            if (_loggedInUser != null && _loggedInUser.Role == "Administratorius")
            {
                return RedirectToAction("Index", "ImoniuAdministravimas");
            }
            else
            {
                return RedirectToAction("Index", "Ikelimas");
            }
        }

    }
}
