using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Oracle.DataAccess.Client;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Services;

namespace StatistinesAtaskaitos.Controllers
{
    [Authorize(Roles = "Administratorius")]
    public class StatistineAtaskaitaController : Controller
    {
        private readonly IStatistiniuAtaskaituService _statistiniuAtaskaituService;

        public StatistineAtaskaitaController(IStatistiniuAtaskaituService statistiniuAtaskaituService)
        {
            _statistiniuAtaskaituService = statistiniuAtaskaituService;
        }

        //
        // GET: /StatistineAtaskaita/

        public ActionResult Index()
        {
            return RedirectToAction("Ataskaita", new AtaskaitosParametrai
                                                     {
                                                         FormosTipas = FormuTipai.Forma1,
                                                         Metai = DateTime.Now.Year - 1
                                                     });
        }

        public ActionResult Ataskaita(AtaskaitosParametrai model)
        {
            return View(_statistiniuAtaskaituService.GetStatistineAtaskaita(model));
        }
    }
}
