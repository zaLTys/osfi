using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StatistinesAtaskaitos.Models;

namespace StatistinesAtaskaitos.Services
{
    //public class DummyStatistiniuAtaskaituService : IStatistiniuAtaskaituService
    //{
    //    private List<Forma11> _dummyAtaskaita = new List<Forma11>();

    //    public DummyStatistiniuAtaskaituService()
    //    {
    //        _dummyAtaskaita.Add(new Forma11
    //            {
    //                ImonesKodas = 123,
    //                Kodas = 010.ToString(),
    //                Pavadinimas = "Grudai",
    //                LikutisPradziojeIlgalaikio = 38,

    //            });
    //        _dummyAtaskaita.Add(new Forma11
    //            {
    //                ImonesKodas = 123,
    //                Kodas = 011.ToString(),
    //                Pavadinimas = "Ne grudai",
    //                LikutisPradziojeIlgalaikio = 38,
    //            });
    //    }
        
    //    public IEnumerable<Forma11> GetForma1(long imonesKodas)
    //    {
    //        yield return _dummyAtaskaita.FirstOrDefault(x => x.ImonesKodas == imonesKodas);
    //    }



    //    IEnumerable<Forma2> IStatistiniuAtaskaituService.GetForma2(long imonesKodas)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    IEnumerable<Forma3> IStatistiniuAtaskaituService.GetForma3(long imonesKodas)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    IEnumerable<Forma41> IStatistiniuAtaskaituService.GetForma41(long imonesKodas)
    //    {
    //        throw new NotImplementedException();
    //    }
    //    IEnumerable<Forma42> IStatistiniuAtaskaituService.GetForma42(long imonesKodas)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}