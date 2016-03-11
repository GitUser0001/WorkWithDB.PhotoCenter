using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class DeliveryGoods
    {
        // id
        //public Delivery Delivery { get; set; }
        //public Goods Goods { get; set; }
        //\\

        // id
        public int DeliveryID { get; set; }
        public int GoodsID { get; set; }
        //\\

        public int Count { get; set; }
        public float Price { get; set; }

        public DeliveryGoods Clone()
        {
            return (DeliveryGoods)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "DeliveryGoods(DeliveryId: {0}, GoodsId: {1})", DeliveryID, GoodsID);
        }
    }
}
