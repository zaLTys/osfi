using System;

namespace Vic.ZubStatistika.Entities
{
    public class UploadStatus
    {
        public virtual int Id { get; set; }
        public virtual Upload Upload { get; set; }
        public virtual User User { get; set; }
        public virtual string Bukle { get; set; }
        public virtual DateTime DataNuo { get; set; }
        public virtual DateTime? DataIki { get; set; }
    }
}