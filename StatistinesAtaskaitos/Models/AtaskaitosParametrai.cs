using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class AtaskaitosParametrai
    {
        [DataType("Metai")]
        public int? Metai { get; set; }
        public FormuTipai FormosTipas { get; set; }
        public long? ImonesKodas { get; set; }
        public int? UploadId { get; set; }
    }
}