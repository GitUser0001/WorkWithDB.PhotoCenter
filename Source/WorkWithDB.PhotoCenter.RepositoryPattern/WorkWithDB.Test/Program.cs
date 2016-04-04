using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL;
using Model = WorkWithDB.DAL.Entity.Entities;
using System.Configuration;

namespace WorkWithDB.Test
{
    public class Program
    {
        private static void Main(string[] args)
        {
            UnitOfWorkFactory.__Initialize(() => new UnitOfWork());

            Console.WriteLine("Connecting...");
            Console.WriteLine(ConfigurationManager.ConnectionStrings["Home"].ConnectionString);
            using (IUnitOfWork scope = UnitOfWorkFactory.CreateInstance())
            {
                Console.WriteLine("Connected");

                Goods goods = new Goods()
                {
                    Barcode = 1323,
                    Cost = 2,
                    CriticalNumber = 2,
                    MadeIN = "AAAAAA",
                    Name = "sssssss"
                };

                int a = scope.GoodsRepository.Save(goods);
                scope.Commit();
                Console.WriteLine("res = " + a.ToString());                
            }

            Console.ReadLine();
        }
    }
}
