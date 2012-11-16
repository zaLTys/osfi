using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vic.ZubStatistika.Entities
{
    public interface IKlasifikatorius
    {
        int Id { get; set; }
        string Kodas { get; set; }
        string Pavadinimas { get; set; }
    }
}
