using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities.Abstract;

namespace WorkWithDB.DAL.Entity.Entities
{
    public class PhotoFormat : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public float PriceOfOne { get; set; }

        public PhotoFormat Clone()
        {
            return (PhotoFormat)base.MemberwiseClone();
        }

        public override string ToString()
        {
            return string.Format(
                "GoodsSold(Id: {1})", Id);
        }
    }
}
