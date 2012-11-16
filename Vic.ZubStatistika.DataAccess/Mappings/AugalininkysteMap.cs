using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using Vic.ZubStatistika.Entities;

namespace Vic.ZubStatistika.DataAccess.Mappings
{
    public class AugalininkysteMap : ClassMap<Augalininkyste>
    {
        public AugalininkysteMap()
        {
            
        }
    }

    public class DarbuotojaiMap : ClassMap<Darbuotojai>
    {
        public DarbuotojaiMap()
        {
            References(x => x.Imone)
                .Not.Nullable();

            References(x => x.Rusis)
                .Not.Nullable();
        }
    }
}
