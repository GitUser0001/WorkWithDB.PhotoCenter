using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class PaperFormat : BaseEntity<int>
    {
        public string Name { get; set; }
        public float PriceOne { get; set; }
        public string Size { get; set; }

        public PaperFormat Clone()
        {
            return (PaperFormat)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Kiosk(Id: {1})", Id);
        }
    }
}
