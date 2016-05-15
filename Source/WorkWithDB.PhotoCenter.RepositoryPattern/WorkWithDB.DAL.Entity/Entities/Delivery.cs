using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Delivery : BaseEntity<int>
    {
        //Link
        public StructuralUnit StructuralUnit { get; set; }
        public Provider Provider { get; set; }
        //\\

        //Link
        //public int StructuralUnitID { get; set; }
        //public int ProviderID { get; set; }
        //\\


        public DateTime Date_order { get; set; }
        public DateTime Date_import { get; set; }        

        public Delivery Clone()
        {
            return (Delivery)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Delivery(Id: {1})", Id);
        }
    }
}
