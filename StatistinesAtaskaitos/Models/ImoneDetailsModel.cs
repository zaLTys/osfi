using System;
using System.Collections.Generic;

namespace StatistinesAtaskaitos.Models
{
    public class ImoneDetailsModel
    {
        public int Id { get; set; }
        public long AsmensKodas { get; set; }
        public string Pavadinimas { get; set; }
        public List<ImoneDetailsUploadModel> Ikelimai { get; set; }
    }

    public class ImoneDetailsUploadModel
    {
        public int Id { get; set; }
        public int Metai { get; set; }
        public DateTime Data { get; set; }
        public string Bukle { get; set; }
        public DateTime BuklesData { get; set; }
    }
}