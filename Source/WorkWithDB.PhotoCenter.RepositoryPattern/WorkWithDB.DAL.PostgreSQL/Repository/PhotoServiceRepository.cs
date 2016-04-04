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
    internal class PhotoServiceRepository : BaseRepository<int, PhotoService>, IPhotoServiceRepository
    {
        public PhotoServiceRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
            : base(connection, transaction)
        { 
        }


        public override int Save(PhotoService entity)
        {
            entity.Id =
                base.ExecuteScalar<int>(
                    @"insert into photo_service (service_id,photo_count,photographer_id,filiya_id,is_immediately,price)
                    values (@service_id,@photo_count,@photographer_id,@filiya_id,@is_immediately,@price) SELECT RETURNING id",
                    new SqlParameters                    
                    {
                        {"service_id", entity.ServiceID},                    
                        {"photo_count", entity.PhotoCount},                    
                        {"photographer_id", entity.Photographer.Id},    
                        {"filiya_id", entity.Filiya.StructureUnitID},                    
                        {"is_immediately", entity.IsImmediately},                    
                        {"price", entity.Price}            
                    });

            return entity.Id;
        }

        public override bool Update(PhotoService entity)
        {
            var res = base.ExecuteNonQuery(
            @"update photo_service set service_id=@service_id,photo_count=@photo_count,
                photographer_id=@photographer_id,filiya_id=@filiya_id,is_immediately=@is_immediately,price=@price
                WHERE id=@id",
                new SqlParameters
                    {
                        {"id", entity.Id},
                        {"service_id", entity.ServiceID},                    
                        {"photo_count", entity.PhotoCount},                    
                        {"photographer_id", entity.Photographer.Id},    
                        {"filiya_id", entity.Filiya.StructureUnitID},                    
                        {"is_immediately", entity.IsImmediately},                    
                        {"price", entity.Price}       
                    });

            return res > 0;
        }

        public int GetCount()
        {
            return base.ExecuteScalar<int>("select count(*) from photo_service");
        }

        public PhotoService GetByID(int id)
        {
            return base.ExecuteSingleRowSelect(
                    "select * from photo_service where id = @id",
                    new SqlParameters()                        
                    {                         
                        {"id", id}
                    });
        }

        public bool Delete(int id)
        {
            var res = base.ExecuteNonQuery(
                        "delete from photo_service where id = @id",
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

        public IList<PhotoService> GetAll()
        {
            return base.ExecuteSelect("select * from photo_service");
        }

        protected override PhotoService DefaultRowMapping(NpgsqlDataReader reader)
        {
            using (var unitOfWork = UnitOfWorkFactory.CreateInstance())
            {
                return new PhotoService
                {
                    Id = (int)reader["id"],
                    ServiceID = (int)reader["service_id"],
                    Filiya = unitOfWork.FiliyaRepository.GetByID((int)reader["filiya_id"]),
                    Photographer = unitOfWork.PhotographerRepository.GetByID((int)reader["photographer_id"]),
                    PhotoCount = (int)reader["photo_count"],
                    IsImmediately = (bool)reader["is_immediately"],
                    Price = (float)reader["price"]
                };
            }
        }
    }
}
