using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class UserModel
    {
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Role { get; set; }
        public virtual string Vardas { get; set; }
        public virtual string Pavarde { get; set; }
    }
}