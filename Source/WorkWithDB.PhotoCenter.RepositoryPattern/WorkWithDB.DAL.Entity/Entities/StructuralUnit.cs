using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class StructuralUnit : BaseEntity<int>
    {
        public string Name { get; set; }
        public string OwnerInfo { get; set; }
        public string Adress { get; set; }
        public DateTime Opening_Date { get; set; }
        public int Jobs { get; set; }

        public StructuralUnit Clone()
        {
            return (StructuralUnit)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format("{0}", Id);
        }
    }
}
