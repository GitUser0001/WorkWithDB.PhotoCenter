using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Service : BaseEntity<int>
    {
        // Link
        public Order Order { get; set; }
        public ServiceType Type { get; set; }
        //\\

        // Link
        //public int OrderID { get; set; }
        //public int TypeID { get; set; }
        //\\

        public Service Clone()
        {
            return (Service)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Service(OrderId: {1})", Id);
        }
    }
}
