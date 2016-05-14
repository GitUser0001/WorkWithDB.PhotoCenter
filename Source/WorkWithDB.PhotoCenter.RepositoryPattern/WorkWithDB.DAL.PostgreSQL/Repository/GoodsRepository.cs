using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;
using WorkWithDB.DAL.Abstract.Repository;
using Npgsql;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    internal class GoodsRepository : BaseRepository<int, Goods>, IGoodsRepository
    {
        public GoodsRepository(NpgsqlConnection connection, NpgsqlTransaction transaction) : base(connection, transaction)
        {
        }       
        
        public override int Save(Goods entity)
        {
            entity.Id = base.ExecuteScalar<int>(
                    @"insert into goods (name,made_in,barcode,cost,critical_number)
                    values (@name,@made_in,@barcode,@cost,@critical_number) RETURNING id",
                    new SqlParameters                    
                    {                   
                        {"name", entity.Name},                    
                        {"made_in", entity.MadeIN},    
                        {"barcode", entity.Barcode},                    
                        {"cost", entity.Cost},                    
                        {"critical_number", entity.CriticalNumber}               
                    });

            return entity.Id;
        }

        public override bool Update(Goods entity)
        {
            var res = base.ExecuteNonQuery(
            @"update goods set name=@name,made_in=@made_in,barcode=@barcode,cost=@cost,critical_number=@critical_number
                WHERE id = @id",
                new SqlParameters
                    {
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"made_in", entity.MadeIN},    
                        {"barcode", entity.Barcode},                    
                        {"cost", entity.Cost},                    
                        {"critical_number", entity.CriticalNumber}  
                    });

            return res > 0;
        }

        public IList<Goods> GetByCountry(string countryName)
        {
            return base.ExecuteSelect(
                    "select * from goods where made_id = @id",
                    new SqlParameters()                        
                    {
                        {"id", countryName}
                    });
        }

        public int GetCountByCountry(string countryName)
        {
            return (int)
                base.ExecuteScalar<long>(
                    "select count(*) from goods where made_in = @madeIn",
                    new SqlParameters()                        
                    {
                        {"madeIn", countryName}
                    });
        }

        public Goods GetByBarcode(int barcode)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from goods where barcode = @id",
                    new SqlParameters()                        
                    {
                        {"id", barcode}
                    });
        }

        public int GetCount()
        {
            return (int)
                base.ExecuteScalar<long>("select count(*) from goods");
        }

        public Goods GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from goods where id = @id",
                    new SqlParameters()                        
                    {
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from goods where id = @id",
                        new SqlParameters()
                        {
                            {"id", id}
                        });

            if (res > 1)
            {
                throw new InvalidOperationException("Multiple rows deleted by single delete query");
            }

            return res == 1;
        }

        public IList<Goods> GetAll()
        {
            return base.ExecuteSelect("select * from goods");
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
