using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Goods : BaseEntity<int>
    {
        public string Name { get; set; }
        public string MadeIN { get; set; }
        public int Barcode { get; set; }
        public int Cost { get; set; }
        public int CriticalNumber { get; set; }

        public Goods Clone()
        {
            return (Goods)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Goods(Id: {1}, Name: {2}, Cost: {3}UAH, MadeIN: {4}, Critical Number: {5}, Barcode: {6})", 
                null, 
                Id, 
                Name, 
                Cost, 
                MadeIN, 
                CriticalNumber, 
                Barcode);
        }
    }
}
