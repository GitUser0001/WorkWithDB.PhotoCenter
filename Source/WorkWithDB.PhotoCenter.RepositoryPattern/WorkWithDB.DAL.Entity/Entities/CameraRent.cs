using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class CameraRent
    {
        //id
        //public Service Service { get; set; }
        //\\
        
        // Link
        //public Camera Camera { get; set; }
        //public Filiya Filiya { get; set; }
        //\\

        //id
        public int ServiceID { get; set; }
        //\\

        // Link
        public int CameraID { get; set; }
        public int FiliyaID { get; set; }
        //\\

        public float Price { get; set; }
        public string PassportNumber { get; set; }
        public string PassportCode { get; set; }
        public DateTime PeriodOfTime { get; set; }


        public CameraRent Clone()
        {
            return (CameraRent)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format("CameraRent(ServiceId: {1})", ServiceID);
        }
    }
}
