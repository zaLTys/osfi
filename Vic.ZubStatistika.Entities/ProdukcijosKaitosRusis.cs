﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class ProdukcijosKaitosRusis : IKlasifikatorius
    {
        public virtual int Id { get; set; }
        public virtual string Kodas { get; set; }
        public virtual string Pavadinimas { get; set; }
        public virtual int Metai { get; set; }

        public override string ToString()
        {
            return string.Format("ProdukcijosKaitosRusis[Id={0}, Kodas={1}, Pavadinimas={2}]", Id, Kodas, Pavadinimas);
        }
    }
}
