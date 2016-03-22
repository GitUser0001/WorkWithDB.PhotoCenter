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
        private readonly string tabelName = "photographer";

        public PhotographerRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        {
        }
        
        public override int Save(Photographer entity)
        {
            return (int)
                base.ExecuteScalar<double>(
                    @"insert into @tableName (id,info,photo_price)
                    values (@id,@info,@photo_price) SELECT SCOPE_IDENTITY()",
                    new SqlParameters                    
                    {          
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"info", entity.Info},                    
                        {"photo_price", entity.PhotoPrice}                   
                    });
        }

        public override bool Update(Photographer entity)
        {
            var res = base.ExecuteNonQuery(
            @"update @tableName set id=@id,info=@info,photo_price=@photo_price",
                new SqlParameters
                    {
                        {"tableName", tabelName},
                        {"id", entity.Id},                    
                        {"info", entity.Info},                    
                        {"photo_price", entity.PhotoPrice}
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

        public Photographer GetByID(int id)
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

        public IList<Photographer> GetAll()
        {
            return base.ExecuteSelect(
                "select * from @tableName",
                new SqlParameters 
                { 
                    {"tableName", tabelName}
                });
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
