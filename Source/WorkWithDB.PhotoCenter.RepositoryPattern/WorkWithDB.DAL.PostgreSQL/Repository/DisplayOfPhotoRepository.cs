using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    public class DisplayOfPhotoRepository : BaseRepository<int, DisplayOfPhoto>, IDisplayOfPhotoRepository
    {

    }
}
