using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Mapping;
using Vic.ZubStatistika.Entities;

namespace Vic.ZubStatistika.DataAccess.Mappings
{
    public class KlaidosAprasasMap : IAutoMappingOverride<KlaidosAprasas>
    {
        public void Override(AutoMapping<KlaidosAprasas> mapping)
        {
            mapping.Table("Klaidos");
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
