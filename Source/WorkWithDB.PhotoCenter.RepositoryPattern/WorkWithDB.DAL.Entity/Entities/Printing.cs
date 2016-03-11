using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Printing : BaseEntity<int>
    {
        // Id
        //public Service Service { get; set; }
        //\\

        // Link
        //public PhotoFormat PhotoFormat { get; set; }
        //public PaperFormat PaperFormat { get; set; } 
        //\\


        // Id
        public int ServiceID { get; set; }
        //\\

        // Link
        public int PhotoFormatID { get; set; }
        public int PaperFormatID { get; set; }
        //\\


        public float Price { get; set; }
        public int CopyCount { get; set; }
        public int Discount { get; set; }
        public bool IsImmediately { get; set; }       

        public Printing Clone()
        {
            return (Printing)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Printing(ServiceId: {1})", ServiceID);
        }
    }
}
