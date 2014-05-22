﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public class Kapitalas : IExcelioLentele<KapitaloRusis>, IStatistineAtaskaita
    {
        public virtual int Id { get; set; }
        public virtual Imone Imone { get; set; }
        public virtual KapitaloRusis Rusis { get; set; }
        public virtual decimal FinMetai { get; set; }
        public virtual decimal PraejeFinMetai { get; set; }
        public virtual Upload Upload { get; set; }
    }
}
