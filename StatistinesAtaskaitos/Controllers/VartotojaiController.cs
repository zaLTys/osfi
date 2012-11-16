using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Security;

namespace StatistinesAtaskaitos.Controllers
{
    public class VartotojaiController : Controller
    {
        private readonly IAuthenticationProvider _authenticationProvider;

        public VartotojaiController(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider;
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
    }
}
