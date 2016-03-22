using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.DAL.Abstract.Repository
{
    public interface IAvailabilityRepository : IBaseRepository<int, Availability>
    {
        IList<Availability> GetByGoodsID(int goodsId);

        IList<Availability> GetBySrtUnitID(int strUnitId);
    }
}
