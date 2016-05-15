using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Views
{
    public class ServiceUnionUrgency : Service
    {
        public double Price { get; set; }
        public bool IsImmediately { get; set; }

        public ServiceUnionUrgency Clone()
        {
            return (ServiceUnionUrgency)base.MemberwiseClone();
        }
    }
}
