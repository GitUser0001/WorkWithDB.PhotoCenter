using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Kiosk
    {
        //id
        //public StructuralUnit StructureUnit { get; set; }
        //\\

        //Link        
        //public Filiya Filiya { get; set; }
        //\\

        //id
        public int StructureUnitID { get; set; }
        //\\

        //Link        
        public int FiliyaID { get; set; }
        //\\

        public Kiosk Clone()
        {
            return (Kiosk)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Kiosk(StructuralUnitId: {1})", StructureUnitID);
        }
    }
}
