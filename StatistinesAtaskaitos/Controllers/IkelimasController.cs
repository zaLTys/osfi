using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatistikosFormos;
using StatistinesAtaskaitos.Models;
using StatistinesAtaskaitos.Security;
using StatistinesAtaskaitos.Services;
using Vic.ZubStatistika.Entities;

namespace StatistinesAtaskaitos.Controllers
{
    public class IkelimasController : Controller
    {
        private static log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExcelImporter _excelImporter;
        private readonly IStatistiniuAtaskaituService _statistiniuAtaskaituService;
        private readonly UserInformation _loggedInUser;

        public IkelimasController(ExcelImporter excelImporter, IStatistiniuAtaskaituService statistiniuAtaskaituService, [LoggedIn] UserInformation loggedInUser)
        {
            _excelImporter = excelImporter;
            _statistiniuAtaskaituService = statistiniuAtaskaituService;
            _loggedInUser = loggedInUser;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new UploadModel
                            {
                                Metai = DateTime.Now.Year - 1,
                            });
        }

        [HttpPost]
        public ActionResult Index(UploadModel model)
        {
            try
            {
                if (_loggedInUser == null) model.Metai = DateTime.Now.Year - 1;

                // Verify that the user selected a file
                if (model.File != null && model.File.ContentLength > 0)
                {
                    // extract only the fielname
                    var taskoIndexas = model.File.FileName.LastIndexOf('.');
                    if (taskoIndexas < 0) return View("UploadError", model: "Byla privalo turėti išplėtimą 'xls' arba 'xlsx'");

                    var extension = model.File.FileName.Substring(taskoIndexas + 1);
                    var fileName = model.Id.ToString("N") + '.' + extension;
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    model.File.SaveAs(path);

                    var upload = _excelImporter.Import(path, model.Metai, model.Id);
                    Session["lastUpload"] = upload.Id;

                    return View("UploadSuccess");
                }

                return View("UploadError", model: "Klaida įkeliant bylą");
            }
            catch (Exception ex)
            {
                Log.Error("Klaida įkeliant bylą", ex);
                return View("UploadError", model: "Klaida įkeliant bylą");
            }
        }

        public ActionResult LastUpload(FormuTipai? formosTipas)
        {
            formosTipas = formosTipas ?? FormuTipai.Forma1;
            var lastUpload = Session["lastUpload"];
            int uploadId;
            if (lastUpload != null && int.TryParse(lastUpload.ToString(), out uploadId))
            {
                var ataskaitosParametrai = new AtaskaitosParametrai
                                           {
                                               UploadId = uploadId,
                                               FormosTipas = formosTipas.Value
                                           };

                return View(_statistiniuAtaskaituService.GetStatistineAtaskaita(ataskaitosParametrai));
            }

            return RedirectToAction("Index");
        }

    }
}
