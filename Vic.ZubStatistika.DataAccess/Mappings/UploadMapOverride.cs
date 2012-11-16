using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Vic.ZubStatistika.Entities;

namespace Vic.ZubStatistika.DataAccess.Mappings
{
    public class UploadMapOverride : IAutoMappingOverride<Upload>
    {
        public void Override(AutoMapping<Upload> mapping)
        {
            mapping.HasMany(x => x.Bukles)
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.IlgalaikisTurtas)
                .Inverse()
                .KeyColumn("UPLOAD_ID");
        }
    }
    
    /*
    public class IlgalaikisTurtasMapOverride : IAutoMappingOverride<IlgalaikisTurtas>
    {
        public void Override(AutoMapping<IlgalaikisTurtas> mapping)
        {
            mapping.HasOne(x => x.Imone)
                .Fetch.Join();

            mapping.References(x => x.Upload)
                .Column("UPLOAD_ID");
        }
    }*/

    public class UserMapOverride : IAutoMappingOverride<User>
    {
        public void Override(AutoMapping<User> mapping)
        {
            mapping.Table("Users");

        }
    }

}