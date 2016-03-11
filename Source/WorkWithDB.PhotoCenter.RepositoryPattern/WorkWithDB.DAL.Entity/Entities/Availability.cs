using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Availability
    {
        //id
        //public Goods Goods { get; set; }
        //public StructuralUnit SrtucturalUnit { get; set; }
        //\\

        //id
        public int StructuralUnitID { get; set; }
        public int GoodsID { get; set; }
        //\\

        public int Count { get; set; }

        public Availability Clone()
        {
            return (Availability)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Availability(GoodsID: {1}, StructuralUnitID: {2}, Count: {3})", GoodsID, StructuralUnitID, Count);
        }
    }
}
