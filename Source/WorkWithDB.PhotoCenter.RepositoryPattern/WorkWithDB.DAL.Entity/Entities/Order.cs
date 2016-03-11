using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Order : BaseEntity<int>
    {
        // Link
        //public Client Client { get; set; }
        //public DateTime SoldDate { get; set; }
        //public StructuralUnit StructureUnit { get; set; }
        //\\

        // Link
        public int ClientID { get; set; }
        public int SoldDateID { get; set; }
        public int StructureUnitID { get; set; }
        //\\

        public Order Clone()
        {
            return (Order)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Order(Id: {1})", Id);
        }
    }
}
