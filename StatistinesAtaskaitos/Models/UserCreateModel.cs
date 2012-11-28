using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class UserCreateModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
        public RoliuTipai Role { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
    }

    public class UserEditModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public RoliuTipai Role { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
    }
}