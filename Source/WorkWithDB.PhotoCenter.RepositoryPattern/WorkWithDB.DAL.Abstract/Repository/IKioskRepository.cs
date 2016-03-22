using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.DAL.Abstract.Repository
{
    public interface IKioskRepository : IBaseRepository<int, Kiosk>
    {
    }
}
