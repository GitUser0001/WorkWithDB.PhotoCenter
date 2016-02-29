using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithDB.DAL.Entity.Entities.Abstract
{
    public abstract class BaseEntity<Tkey>
    {
        public Tkey Id { get; set; }
    }
}
