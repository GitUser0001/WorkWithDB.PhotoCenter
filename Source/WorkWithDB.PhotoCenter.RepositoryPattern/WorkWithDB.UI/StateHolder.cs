using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.UI
{
    public static class StateHolder
    {
        public static Model.Client RegistratingClient 
        {
            get
            {
                var a = RegistratingClient;
                RegistratingClient = null;
                return a;
            } 
            set
            {
                RegistratingClient = value;
            }
        }
    }
}
