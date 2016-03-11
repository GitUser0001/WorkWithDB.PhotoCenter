using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;
using WorkWithDB.DAL.Abstract.Repository;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    class FilmTypeRepository : BaseRepository<int, FilmType>, IFilmTypeRepository
    {
    }
}
