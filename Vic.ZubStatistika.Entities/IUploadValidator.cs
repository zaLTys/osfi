using System.Collections.Generic;

namespace Vic.ZubStatistika.Entities
{
    public interface IUploadValidator
    {
        IEnumerable<KlaidosAprasas> Validate(Upload upload);
    }
}
