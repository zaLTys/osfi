using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class Forma42IrFormuPildymoLaikas
    {
        public IEnumerable<Forma42> Forma42 { get; set; }
        public FormosPildymoLaikasModel FormosPildymoLaikas { get; set; }
    }
}