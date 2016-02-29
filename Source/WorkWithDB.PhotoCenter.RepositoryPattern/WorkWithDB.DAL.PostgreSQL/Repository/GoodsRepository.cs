using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;
using WorkWithDB.DAL.Abstract;
using Npgsql;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    internal class GoodsRepository : BaseRepository<int, Goods>, IGoodsRepository
    {
        public GoodsRepository(NpgsqlConnection connection, NpgsqlTransaction transaction) : base(connection, transaction)
        {
        }

        public IList<Goods> GetAll()
        {
            return base.ExecuteSelect("SELECT id, name, made_in, barcode, cost, critical_number FROM \"BD_LAB\".goods;");
        }

        public IList<Goods> GetByCountry(string countryName)
        {
            throw new NotImplementedException();
        }

        public int GetCountByCountry(string countryName)
        {
            throw new NotImplementedException();
        }

        public int Insert(Goods entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Goods entity)
        {
            throw new NotImplementedException();
        }

        public int Upsert(Goods entity)
        {
            throw new NotImplementedException();
        }

        public int GetCount()
        {
            throw new NotImplementedException();
        }

        public Goods GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        protected override Goods DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new Goods
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                MadeIN = (string)reader["made_in"],
                Barcode = (int)reader["barcode"],
                Cost = (int)reader["cost"],
                CriticalNumber = (int)reader["critical_number"]
            };
        }
    }
}
