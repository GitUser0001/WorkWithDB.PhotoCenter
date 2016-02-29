using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.PostgreSQL;

namespace WorkWithDB.Test
{
    public class Program
    {
        private static void Main(string[] args)
        {
            using (IUnitOfWork scope = new UnitOfWork())
            {
                var listOfGoods = scope.GoodsRepository.GetAll();

                int i = 0;

                foreach (var goods in listOfGoods)
                {
                    Console.WriteLine(goods.ToString());
                    i++;

                    if (i > 10)
                    {
                        break;
                    }
                }

                Console.ReadLine();
            }
        }
    }
}
