using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class GoodsCountViewModel
    {
        public string EntityName 
        {
            get { return "Goods"; }
        }
        public string CountOf { get; set; }
        public int Count { get; set; }

        public GoodsCountViewModel(int count, string countOf = "All")
        {
            Count = count;
            CountOf = countOf;
        }
    }
}