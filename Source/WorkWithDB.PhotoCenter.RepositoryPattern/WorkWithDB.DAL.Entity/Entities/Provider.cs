using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Provider : BaseEntity<int>
    {
        public int MobileNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string OwnerInfo { get; set; }

        public Provider Clone()
        {
            return (Provider)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Provider(Id: {1})", Id);
        }
    }
}
