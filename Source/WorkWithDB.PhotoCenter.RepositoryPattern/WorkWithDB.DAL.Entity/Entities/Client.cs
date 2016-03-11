using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class Client : BaseEntity<int>
    {
        //Link
        //public DiscountCard DiscountCard { get; set; }
        //\\

        //Link
        public int DiscountCardID { get; set; }
        //\\

        public DateTime RegistrationDate { get; set; }
        public string Full_name { get; set; }
        public bool Is_profesional { get; set; }

        public Client Clone()
        {
            return (Client)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format("Client(Id: {1})", Id);
        }
    }
}
