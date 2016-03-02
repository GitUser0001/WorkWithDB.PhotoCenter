using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract.Repository;

namespace WorkWithDB.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IGoodsRepository GoodsRepository { get; }

        void Commit();
        void RollBack();
    }
}
