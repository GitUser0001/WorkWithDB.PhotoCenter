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
        private readonly string tabelName = "goods";

        public GoodsRepository(NpgsqlConnection connection, NpgsqlTransaction transaction) : base(connection, transaction)
        {
        }       
        
        public override int Save(Goods entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,name,made_in,barcode,cost,critical_number)
                    values (@id,@name,@made_in,@barcode,@cost,@critical_number) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id@", entity.Id},                    
                        {"name", entity.Name},                    
                        {"made_in", entity.MadeIN},    
                        {"barcode", entity.Barcode},                    
                        {"cost", entity.Cost},                    
                        {"critical_number", entity.CriticalNumber}               
                    });
        }

        public override bool Update(Goods entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,name=@name,made_in=@made_in,barcode=@barcode,cost=@cost,critical_number=@critical_number",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id@", entity.Id},                    
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
                    "select * from @tableName where made_id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", countryName}
                    });
        }

        public int GetCountByCountry(string countryName)
        {
            return base.ExecuteScalar<int>(
                    "select count(*) from @tableName where made_id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", countryName}
                    });
        }

        public Goods GetByBarcode(int barcode)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from @tableName where barcode = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", barcode}
                    });
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>(
                        "select count(*) from @tableName",
                        new SqlParameters 
                        { 
                            {"tableName", tabelName}
                        });
        }

        public Goods GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from @tableName where id = @id",
                    new SqlParameters()                        
                    {
                        {"tableName", tabelName},
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from @tableName where id = @id",
                        new SqlParameters()
                        {
                            {"tableName", tabelName},
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
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
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
