using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Photographer : BaseEntity<int>
    {
        public string Info { get; set; }
        public float PhotoPrice { get; set; }

        public Photographer Clone()
        {
            return (Photographer)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "Photographer(Id: {1})", Id);
        }
    }
}
