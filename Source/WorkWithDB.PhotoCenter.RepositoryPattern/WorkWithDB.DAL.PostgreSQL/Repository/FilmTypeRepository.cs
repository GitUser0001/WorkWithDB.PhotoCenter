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
        public FilmTypeRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(FilmType entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into film_type (name,length,price)
                    values (@name,@length,@price) RETURNING id",
                    new SqlParameters                    
                    {                   
                        {"name", entity.Name},                    
                        {"length", entity.Lenght},
                        {"price", entity.Price}
                    });

            return entity.Id;
        }

        public override bool Update(FilmType entity)
        {
            var res = base.ExecuteNonQuery(
            @"update film_type set name=@name,length=@length,price=@price
                WHERE id=@id",
                new SqlParameters
                    {                         
                        {"id", entity.Id},                    
                        {"name", entity.Name},                    
                        {"length", entity.Lenght},
                        {"price", entity.Price}
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from film_type");
        }

        public FilmType GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from film_type where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from film_type where id = @id",
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

        public IList<FilmType> GetAll()
        {
            return base.ExecuteSelect("select * from film_type");
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
