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
        public Guid Id { get; set; }
        [DataType("Metai")]
        [Display(Name = "Metai:")]
        public int Metai { get; set; }
        public HttpPostedFileBase File { get; set; }

        public UploadModel()
        {
            Id = Guid.NewGuid();
        }
    }
}