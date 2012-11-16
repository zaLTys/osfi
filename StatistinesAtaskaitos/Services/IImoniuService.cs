using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatistinesAtaskaitos.Models;

namespace StatistinesAtaskaitos.Services
{
    public interface IImoniuService
    {
        IEnumerable<ImoneGridModel> GetImones(int metai);
        ImoneDetailsModel GetImoneDetails(int imonesId, int metai);
    }
}
