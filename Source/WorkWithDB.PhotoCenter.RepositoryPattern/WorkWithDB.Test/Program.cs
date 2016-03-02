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
            UnitOfWorkFactory.__Initialize(() => new UnitOfWork());

            using (IUnitOfWork scope = UnitOfWorkFactory.CreateInstance())
            {                
                var listOfGoods = scope.GoodsRepository.GetAll();

                Console.WriteLine("Elements in Goods table = " + listOfGoods.Count + "\n");
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
