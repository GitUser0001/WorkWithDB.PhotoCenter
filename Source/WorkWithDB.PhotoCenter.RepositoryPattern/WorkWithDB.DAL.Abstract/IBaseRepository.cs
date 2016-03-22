using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Abstract
{
    public interface IBaseRepository<Tkey, TEntity> where TEntity : BaseEntity<Tkey>
    {
        Tkey Save(TEntity entity);
        bool Update(TEntity entity);
        Tkey SaveOrUpdate(TEntity entity);


        int GetCount();


        TEntity GetByID(Tkey id);
        bool Delete(Tkey id);

        IList<TEntity> GetAll();
    }
}
