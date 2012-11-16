using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatistikosFormos;
using StatistinesAtaskaitos.Models;

namespace StatistinesAtaskaitos.Controllers
{
    public class IkelimasController : Controller
    {
        private readonly ExcelImporter _excelImporter;

        public IkelimasController(ExcelImporter excelImporter)
        {
            _excelImporter = excelImporter;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new UploadModel
                            {
                                Metai = DateTime.Now.Year,
                            });
        }

        [HttpPost]
        public ActionResult Index(UploadModel model)
        {
            // Verify that the user selected a file
            if (model.File != null && model.File.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(model.File.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                model.File.SaveAs(path);

                _excelImporter.Import(path, model.Metai);
            }

            return RedirectToAction("Index");
        }

    }
}
