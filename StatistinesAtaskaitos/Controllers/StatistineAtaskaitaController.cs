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
                                                         Metai = DateTime.Now.Year
                                                     });
        }

        public ActionResult Ataskaita(AtaskaitosParametrai model)
        {
            object rezultatai = null;


            switch (model.FormosTipas)
            {
                case FormuTipai.Forma1:
                    rezultatai = _statistiniuAtaskaituService.GetForma1(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma2:
                    rezultatai = _statistiniuAtaskaituService.GetForma2(model.Metai, model.ImonesKodas, model.UploadId);//////////////////////////////////////////
                    break;
                case FormuTipai.Forma3:
                    rezultatai = _statistiniuAtaskaituService.GetForma3(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma41:
                    var imones41 = _statistiniuAtaskaituService.GetForma41(model.Metai, model.ImonesKodas, model.UploadId);
                    var pildymolaikas = _statistiniuAtaskaituService.GetFormosPildymoLaikas(model.Metai, model.ImonesKodas, model.UploadId);
                    rezultatai = new Forma41IrFormuPildymoLaikas
                                     {
                                         Forma41 = imones41,
                                         FormosPildymoLaikas = pildymolaikas,
                                     };
                    break;
                case FormuTipai.Forma42:
                    rezultatai = _statistiniuAtaskaituService.GetForma42(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma5:
                    rezultatai = _statistiniuAtaskaituService.GetForma5(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma6:
                    rezultatai = _statistiniuAtaskaituService.GetForma6(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma7:
                    rezultatai = _statistiniuAtaskaituService.GetForma7(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma8:
                    rezultatai = _statistiniuAtaskaituService.GetForma8(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
                case FormuTipai.Forma9:
                    rezultatai = _statistiniuAtaskaituService.GetForma9(model.Metai, model.ImonesKodas, model.UploadId);
                    break;
            }

            return View(new AtaskaitaModel
                       {
                           Parametrai = model,
                           Rezultatai = rezultatai
                       });
        }
    }
}
