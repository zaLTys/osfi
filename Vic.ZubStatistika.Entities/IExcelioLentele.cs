using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public interface IExcelioLentele<T>
        where T : IKlasifikatorius
    {
        int Id { get; set; }
        Imone Imone { get; set; }
        T Rusis { get; set; }
        Upload Upload { get; set; }
    }
}
