using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class DisplayOfPhoto : BaseEntity<int>
    {
        //id
        //public Service Service { get; set; }
        //\\
        
        //Link
        public FilmType FilmType { get; set; }
        //\\

        //id
        public int ServiceID { get; set; }
        //\\

        //Link
        //public int FilmTypeID { get; set; }
        //\\

        public float Price { get; set; }
        public bool IsWeSold { get; set; }
        public bool IsImmediatelly { get; set; }

        public DisplayOfPhoto Clone()
        {
            return (DisplayOfPhoto)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "DisplayOfPhoto(ServiceId: {1})", ServiceID);
        }
    }
}
