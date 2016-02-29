using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithDB.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IGoodsRepository GoodsRepository { get; }

        void Commit();
        void Rollback();
    }
}
