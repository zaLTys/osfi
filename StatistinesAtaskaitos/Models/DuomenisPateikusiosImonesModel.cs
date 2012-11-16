using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class DuomenisPateikusiosImonesModel
    {
        [Display(Name = "Metai:")]
        [DataType("Metai")]
        public int Metai { get; set; }
        public IEnumerable<ImoneGridModel> Imones { get; set; }
    }
}