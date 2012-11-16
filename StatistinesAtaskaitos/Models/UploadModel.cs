using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatistinesAtaskaitos.Models
{
    public class UploadModel
    {
        [DataType("Metai")]
        public int Metai { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}