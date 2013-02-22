using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using Vic.ZubStatistika.Entities;

namespace Vic.ZubStatistika.DataAccess.Mappings
{
    public class ImonesDuomenysMapOverride : IAutoMappingOverride<ImonesDuomenys>
    {
        public void Override(AutoMapping<ImonesDuomenys> mapping)
        {
            mapping.References(x => x.Upload)
                .Column("UPLOAD_ID");
        }
    }

    public class UploadStatusMapOverride : IAutoMappingOverride<UploadStatus>
    {
        public void Override(AutoMapping<UploadStatus> mapping)
        {
            mapping.Map(x => x.DataNuo).Column("DATA_NUO");
            mapping.Map(x => x.DataIki).Column("DATA_IKI");
        }
    }

    public class UploadMapOverride : IAutoMappingOverride<Upload>
    {
        public void Override(AutoMapping<Upload> mapping)
        {
            mapping.HasMany(x => x.Bukles)
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.IlgalaikisTurtas)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.Augalininkyste)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.Darbuotojai)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.DotacijosSubsidijos)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.FormosPildymoLaikas)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.Gyvulininkyste)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.GyvuliuSkaicius)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.ImonesDuomenys)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.ProdukcijosKaita)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.ProduktuPardavimas)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.Sanaudos)
                .Inverse()
                .KeyColumn("UPLOAD_ID");

            mapping.HasMany(x => x.ZemesPlotai)
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