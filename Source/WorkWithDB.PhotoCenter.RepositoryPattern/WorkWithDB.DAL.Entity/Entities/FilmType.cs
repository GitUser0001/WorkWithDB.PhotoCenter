using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class FilmType : BaseEntity<int>
    {
        public string Name { get; set; }
        public float Lenght { get; set; }
        public float Price { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Kiosk(Id: {1})", Id);
        }
    }
}
