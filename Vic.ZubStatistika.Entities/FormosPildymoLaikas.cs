using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class FormosPildymoLaikas : IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual int Valandos { get; set; }
        public virtual int Minutes { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
