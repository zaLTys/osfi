using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public RoliuTipai Role { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
    }
}