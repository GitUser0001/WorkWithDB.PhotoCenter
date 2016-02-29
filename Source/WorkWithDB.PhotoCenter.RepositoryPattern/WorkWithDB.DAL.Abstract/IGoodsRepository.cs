using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.DAL.Abstract
{
    public interface IGoodsRepository : IBaseRepository<int, Goods>
    {
        IList<Goods> GetByCountry(string countryName);
        int GetCountByCountry(string countryName);
    }
}
