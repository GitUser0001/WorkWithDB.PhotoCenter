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
    internal class FilmTypeRepository : BaseRepository<int, FilmType>, IFilmTypeRepository
    {
        private readonly string tabelName = "film_type";

        public FilmTypeRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(FilmType entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,name,length,price)
                    values (@id,@name,@length,@price) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"length", entity.Lenght},
                        {"price", entity.Price}
                    });
        }

        public override bool Update(FilmType entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,name=@name,length=@length,price=@price",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"length", entity.Lenght},
                        {"price", entity.Price}
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

        public FilmType GetByID(int id)
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

        public IList<FilmType> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
        }

        protected override FilmType DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new FilmType
            {
                Id = (int)reader["id"],
                Name = (string)reader["name"],
                Lenght = (float)reader["length"],
                Price = (float)reader["price"]
            };
        }
    }
}
