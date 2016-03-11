using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class PhotoService : BaseEntity<int>
    {
        // id
        //public Service Service { get; set; }
        //\\

        // Link
        //public Photographer Photographer { get; set; }
        //public Filiya Filiya { get; set; }
        //\\

        // id
        public int ServiceID { get; set; }
        //\\

        // Link
        public int PhotographerID { get; set; }
        public int FiliyaID { get; set; }
        //\\

        public int PhotoCount { get; set; }
        public bool IsImmediately { get; set; }

        public PhotoService Clone()
        {
            return (PhotoService)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "PhotoService(Id: {1})", Id);
        }
    }
}
