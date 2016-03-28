using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.PostgreSQL;
using Model = WorkWithDB.DAL.Entity.Entities;

namespace WorkWithDB.Test
{
    public class Program
    {
        private static void Main(string[] args)
        {
            UnitOfWorkFactory.__Initialize(() => new UnitOfWork());

            using (IUnitOfWork scope = UnitOfWorkFactory.CreateInstance())
            {
                Model.Filiya st = new Model.Filiya()
                {
                    Id = 2,
                    StructureUnitID = 1
                };

                int a = scope.FiliyaRepository.Save(st);

                //Console.ReadLine();
            }
        }
    }
}
