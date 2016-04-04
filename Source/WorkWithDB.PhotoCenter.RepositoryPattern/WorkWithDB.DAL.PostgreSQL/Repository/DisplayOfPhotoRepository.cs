using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWithDB.DAL.Abstract;
using WorkWithDB.DAL.Abstract.Repository;
using WorkWithDB.DAL.Entity.Entities;
using WorkWithDB.DAL.PostgreSQL.Infrastructure;

namespace WorkWithDB.DAL.PostgreSQL.Repository
{
    internal class DisplayOfPhotoRepository : BaseRepository<int, DisplayOfPhoto>, IDisplayOfPhotoRepository
    {
        public DisplayOfPhotoRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }

        public override int Save(DisplayOfPhoto entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into display_of_photo (service_id,price,is_we_sold,film_type_id,is_immediately)
                    values (@service_id,@price,@is_we_sold,@film_type_id,@is_immediately) SELECT RETURNING id",
                    new SqlParameters                    
                    {                    
                        {"service_id", entity.ServiceID},                    
                        {"price", entity.Price},                    
                        {"is_we_sold", entity.IsWeSold},    
                        {"film_type_id", entity.FilmType.Id},                    
                        {"is_immediately", entity.IsImmediatelly},                                    
                    });

            return entity.Id;
        }

        public override bool Update(DisplayOfPhoto entity)
        {
            var res = base.ExecuteNonQuery(
            @"update display_of_photo set service_id=@service_id,price=@price,is_we_sold=@is_we_sold,
                                    film_type_id=@film_type_id,is_immediately=@is_immediately
                                    WHERE id = @id",
                new SqlParameters
                    {       
                        {"id", entity.Id},
                        {"service_id", entity.ServiceID},                    
                        {"price", entity.Price},                    
                        {"is_we_sold", entity.IsWeSold},    
                        {"film_type_id", entity.FilmType.Id},                    
                        {"is_immediately", entity.IsImmediatelly},      
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from display_of_photo");
        }

        public DisplayOfPhoto GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from display_of_photo where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from display_of_photo where id = @id",
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

        public IList<DisplayOfPhoto> GetAll()
        {
            return base.ExecuteSelect("select * from display_of_photo");
        }

        protected override DisplayOfPhoto DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new DisplayOfPhoto
                {
                    Id = (int)reader["id"],
                    ServiceID = (int)reader["service_id"],
                    Price = (float)reader["price"],
                    IsWeSold = (bool)reader["is_we_sold"],
                    FilmType = unitOfWork.FilmTypeRepository.GetByID((int)reader["film_type_id"]),
                    IsImmediatelly = (bool)reader["is_immediately"]  
                };
            }
        }
    }
}
