using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Filiya : BaseEntity<int>
    {
        // id
        //public StructuralUnit StructureUnit { get; set; }
        //\\

        // id
        public int StructureUnitID { get; set; }
        //\\

        public Filiya Clone()
        {
            return (Filiya)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Filiya(StructuralUnitId: {1})", StructureUnitID);
        }
    }
}
