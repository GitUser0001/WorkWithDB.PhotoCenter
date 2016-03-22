using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    internal class PaperFormatRepository : BaseRepository<int, PaperFormat>, IPaperFormatRepository
    {
        private readonly string tabelName = "paper_format";

        public PaperFormatRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(PaperFormat entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,name,size,price_of_one)
                    values (@id,@name,@size,@price_of_one) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"size", entity.Size},
                        {"price_of_one", entity.PriceOfOne}
                    });
        }

        public override bool Update(PaperFormat entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,name=@name,size=@size,price_of_one=@price_of_one",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"size", entity.Size},
                        {"price_of_one", entity.PriceOfOne}    
                    });

            return res > 0;
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

        public PaperFormat GetByID(int id)
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

        public IList<PaperFormat> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override PaperFormat DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new PaperFormat
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                Size = (string)reader["size"],
                PriceOfOne = (float)reader["price_of_one"]
            };
        }
    }
}
