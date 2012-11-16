using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Vic.ZubStatistika.DataAccess
{
    public class NHibernateSequenceConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.Sequence(string.Format("Seq_{0}",
                                                        instance.EntityType.Name));
        }
    }
}
