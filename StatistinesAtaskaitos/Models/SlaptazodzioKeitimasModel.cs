using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class SlaptazodzioKeitimasModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Dabartinis slaptažodis:")]
        public string DabartinisSlaptazodis { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Naujas slaptažodis:")]
        public string NaujasSlaptazodis { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Pakartokite naują slaptažodį:")]
        public string PakartotasSlaptazodis { get; set; }
    
    }
}