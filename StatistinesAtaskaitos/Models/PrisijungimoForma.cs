using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class PrisijungimoForma
    {
        [Display(Name = "Vartotojo vardas:")]
        public string VartotojoVardas { get; set; }
        [Display(Name = "Slaptažodis:")]
        [DataType(DataType.Password)]
        public string Slaptazodis { get; set; }
        [Display(Name = "Prisiminti mane")]
        public bool Prisiminti { get; set; }
        public string ReturnUrl { get; set; }
    }
}