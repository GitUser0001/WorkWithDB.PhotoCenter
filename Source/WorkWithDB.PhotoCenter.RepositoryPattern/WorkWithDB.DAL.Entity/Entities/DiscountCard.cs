using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class DiscountCard : BaseEntity<int>
    {
        public string TypeName { get; set; }
        public int Discount { get; set; }
        public int Code { get; set; }
        public bool IsPersonal { get; set; }

        public DiscountCard Clone()
        {
            return (DiscountCard)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "DiscountCard(Id: {0})", Id);
        }
    }
}
