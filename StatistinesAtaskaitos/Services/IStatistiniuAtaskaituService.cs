using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StatistinesAtaskaitos.Models;

namespace StatistinesAtaskaitos.Services
{
    public interface IStatistiniuAtaskaituService
    {
        AtaskaitaModel GetStatistineAtaskaita(AtaskaitosParametrai parametrai);
        IEnumerable<Forma1> GetForma1(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma2> GetForma2(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma3> GetForma3(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma41> GetForma41(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma42> GetForma42(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma5> GetForma5(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma6> GetForma6(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma7> GetForma7(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma8> GetForma8(int? metai, long? imonesKodas, int? uploadId);
        IEnumerable<Forma9> GetForma9(int? metai, long? imonesKodas, int? uploadId);
        FormosPildymoLaikasModel GetFormosPildymoLaikas(int? metai, long? imonesKodas, int? uploadId);

    }

}