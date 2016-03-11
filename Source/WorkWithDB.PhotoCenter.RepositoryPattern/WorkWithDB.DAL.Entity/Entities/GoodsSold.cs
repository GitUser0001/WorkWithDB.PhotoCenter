using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class GoodsSold
    {
        ////id
        //public Service Service { get; set; }
        //public Goods Goods { get; set; }
        ////\\

        //id
        public int ServiceID { get; set; }
        public int GoodsID { get; set; }
        //\\

        public int SoldCount { get; set; }
        public int Price { get; set; }
        

        public GoodsSold Clone()
        {
            return (GoodsSold)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "GoodsSold(ServiceId: {1}, GoodsID: {2})", ServiceID, GoodsID);
        }
    }
}
