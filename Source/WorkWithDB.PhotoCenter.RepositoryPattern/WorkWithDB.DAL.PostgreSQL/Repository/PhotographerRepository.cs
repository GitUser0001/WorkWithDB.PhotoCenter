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
    internal class PhotographerRepository : BaseRepository<int, Photographer>, IPhotographerRepository
    {
        public PhotographerRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }
        
        public override int Save(Photographer entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into photographer (info,photo_price)
                    values (@info,@photo_price) SELECT RETURNING id",
                    new SqlParameters                    
                    {                  
                        {"info", entity.Info},                    
                        {"photo_price", entity.PhotoPrice}                   
                    });

            return entity.Id;
        }

        public override bool Update(Photographer entity)
        {
            var res = base.ExecuteNonQuery(
            @"update photographer set info=@info,photo_price=@photo_price
                WHERE id=@id",
                new SqlParameters
                    {                         
                        {"id", entity.Id},                    
                        {"info", entity.Info},                    
                        {"photo_price", entity.PhotoPrice}
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from photographer");
        }

        public Photographer GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from photographer where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from photographer where id = @id",
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

        public IList<Photographer> GetAll()
        {
            return base.ExecuteSelect("select * from photographer");
        }

        protected override Photographer DefaultRowMapping(NpgsqlDataReader reader)
        {
            return new Photographer
            {                
                Id = (int)reader["id"],
                Info = (string)reader["info"],
                PhotoPrice = (float)reader["photo_price"]
            };
        }
    }
}
