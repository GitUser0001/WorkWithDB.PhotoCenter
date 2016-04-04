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
        public PaperFormatRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(PaperFormat entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into paper_format (name,size,price_of_one)
                    values (@name,@size,@price_of_one) SELECT RETURNING id",
                    new SqlParameters                    
                    {                    
                        {"name", entity.Name},                    
                        {"size", entity.Size},
                        {"price_of_one", entity.PriceOfOne}
                    });

            return entity.Id;
        }

        public override bool Update(PaperFormat entity)
        {
            var res = base.ExecuteNonQuery(
            @"update paper_format set name=@name,size=@size,price_of_one=@price_of_one
                WHERE id=@id",
                new SqlParameters
                    {                         
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"size", entity.Size},
                        {"price_of_one", entity.PriceOfOne}    
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from paper_format");
        }

        public PaperFormat GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from paper_format where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from paper_format where id = @id",
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

        public IList<PaperFormat> GetAll()
        {
            return base.ExecuteSelect("select * from paper_format");
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
