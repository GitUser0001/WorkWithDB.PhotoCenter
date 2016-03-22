using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class ServiceType : BaseEntity<int>
    {
        public string Name { get; set; }

        public ServiceType Clone()
        {
            return (ServiceType)base.MemberwiseClone();
        }
    }
}
